using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Balance.Dto;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Authorization.Users;
using Kuaiyipai.Configuration;
using Kuaiyipai.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kuaiyipai.Auction.Balance
{
    public class BalanceAppService : KuaiyipaiAppServiceBase, IBalanceAppService
    {
        //private const string LoginApi = "https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code";
        private const string OrderApi = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        private const string SandboxOrderApi = "https://api.mch.weixin.qq.com/sandboxnew/pay/unifiedorder";

        private readonly IRepository<UserBalance, long> _balanceRepository;
        private readonly IRepository<UserBalanceRecord, Guid> _balanceRecordRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BalanceAppService(IRepository<UserBalance, long> balanceRepository, IRepository<UserBalanceRecord, Guid> balanceRecordRepository, IRepository<User, long> userRepository, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _balanceRepository = balanceRepository;
            _balanceRecordRepository = balanceRecordRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _appConfiguration = env.GetAppConfiguration();
        }

        public async Task<GetMyBalanceOutputDto> GetMyBalance()
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("没有登录");
            }
            var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
            if (balance == null)
            {
                return new GetMyBalanceOutputDto
                {
                    Total = 0,
                    Frozen = 0,
                    Available = 0
                };
            }
            return new GetMyBalanceOutputDto
            {
                Total = balance.TotalBalance,
                Frozen = balance.FrozenBalance,
                Available = balance.TotalBalance - balance.FrozenBalance
            };
        }

        public async Task<PagedResultDto<GetMyBalanceRecordsOutputDto>> GetMyBalanceRecords(GetMyBalanceRecordsInputDto input)
        {
            var query = _balanceRecordRepository.GetAll().Where(b => b.UserId == AbpSession.UserId);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(b => b.RecordTime);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                .Join(_userRepository.GetAll(), b => b.UserId, u => u.Id, (b, u) => new GetMyBalanceRecordsOutputDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    UserFullName = u.Name,
                    Amount = b.Amount,
                    RecordTime = b.RecordTime,
                    Remarks = b.Remarks
                }).ToListAsync();

            return new PagedResultDto<GetMyBalanceRecordsOutputDto>(count, list);
        }

        [UnitOfWork]
        public async Task<ChargeOutputDto> Charge(ChargeInputDto input)
        {
            var sandbox = Convert.ToBoolean(_appConfiguration["WeChat:Sandbox"]);

            var appId = _appConfiguration["WeChat:AppId"];
            var merchantId = _appConfiguration["WeChat:PayMerchantId"];
            var paymentApiKey = _appConfiguration["WeChat:PaymentApiKey"];
            string sandBoxPaymentApiKey = "";
            //var remoteIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var remoteIp = "192.168.1.1";
            var notifyUrl = new Uri(new Uri(_appConfiguration["App:ServerRootAddress"]), "/api/services/app/Balance/CompleteCharge").ToString();

            if (sandbox)
            {
                var p = new Dictionary<string, string>
                {
                    {"mch_id", merchantId},
                    {"nonce_str", GetRandomString(32, true, false, true, false, "")},
                };
                p.Add("sign", GetSign(p, paymentApiKey));
                var s = new StringBuilder();
                s.Append("<xml>");
                foreach (var d in p)
                {
                    s.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
                }
                s.Append("</xml>");

                var r = await HttpHelper.Post("https://api.mch.weixin.qq.com/sandboxnew/pay/getsignkey", s.ToString());
                DataSet ds1 = new DataSet();
                StringReader stram1 = new StringReader(r);
                XmlTextReader reader1 = new XmlTextReader(stram1);
                ds1.ReadXml(reader1);
                string returnCode1 = ds1.Tables[0].Rows[0]["return_code"].ToString();
                if (returnCode1.ToUpper() == "SUCCESS")
                {
                    sandBoxPaymentApiKey = ds1.Tables[0].Rows[0]["sandbox_signkey"].ToString();
                }
                else
                {
                    throw new UserFriendlyException("沙箱Key获取失败");
                }
            }

            // 获取OpenID
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var user = await _userRepository.GetAsync(AbpSession.UserId.Value);
            if (user == null)
            {
                throw new UserFriendlyException("未找到用户");
            }

            var openId = user.UserName;

            // 创建充值订单记录(余额变化记录)
            var balanceRecord = new UserBalanceRecord
            {
                Id = Guid.NewGuid(),
                Amount = input.Amount,
                RecordTime = DateTime.Now,
                Remarks = "充值",
                UserId = AbpSession.UserId.Value,
                PaymentCompleteTime = new DateTime(1990, 1, 1)
            };

            try
            {
                // 调用统一下单接口
                var paramsDict = new Dictionary<string, string>
                {
                    {"appid", appId},
                    {"mch_id", merchantId},
                    {"nonce_str", GetRandomString(32, true, false, true, false, "")},
                    {"body", "快微拍-余额充值"},
                    {"out_trade_no", balanceRecord.Id.ToString().Replace("-", "").ToUpper()},
                    {"total_fee", (input.Amount * 100).ToString(CultureInfo.InvariantCulture)},
                    {"spbill_create_ip", remoteIp},
                    {"notify_url", notifyUrl},
                    {"trade_type", "JSAPI"},
                    {"openid", openId}
                };
                var sign = !sandbox ? GetSign(paramsDict, paymentApiKey) : GetSign(paramsDict, sandBoxPaymentApiKey);
                paramsDict.Add("sign", sign);
                var sb = new StringBuilder();
                sb.Append("<xml>");
                foreach (var d in paramsDict)
                {
                    sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
                }
                sb.Append("</xml>");

                var result = !sandbox
                    ? await HttpHelper.Post(OrderApi, sb.ToString())
                    : await HttpHelper.Post(SandboxOrderApi, sb.ToString());

                DataSet ds = new DataSet();
                StringReader stram = new StringReader(result);
                XmlTextReader reader = new XmlTextReader(stram);
                ds.ReadXml(reader);
                string returnCode = ds.Tables[0].Rows[0]["return_code"].ToString();
                string prepayId = "";
                if (returnCode.ToUpper() == "SUCCESS")
                {
                    string resultCode = ds.Tables[0].Rows[0]["result_code"].ToString();
                    if (resultCode.ToUpper() == "SUCCESS")
                    {
                        prepayId = ds.Tables[0].Rows[0]["prepay_id"].ToString();
                    }
                }

                // 保存充值订单记录(余额变化记录)
                await _balanceRecordRepository.InsertAndGetIdAsync(balanceRecord);

                // 二次签名返回
                var tick = DateTime.Now.Ticks;
                var nonceStr = GetRandomString(32, true, false, true, false, "");
                var signDict = new Dictionary<string, string>
                {
                    {"appId", appId},
                    {"nonceStr", nonceStr },
                    {"package", "prepay_id=" + prepayId},
                    {"signType", "MD5"},
                    {"timeStamp", tick.ToString()}
                };
                var secondSign = GetSign(signDict, !sandbox ? paymentApiKey : sandBoxPaymentApiKey);

                return new ChargeOutputDto
                {
                    PrepayId = prepayId,
                    Sign = secondSign,
                    NonceStr = nonceStr,
                    TimeStamp = tick.ToString()
                };
            }
            catch
            {
                throw new UserFriendlyException("充值失败");
            }
        }

        /*public async Task<string> CompleteCharge()
        {
            // for test
            var result =
                "<xml><appid><![CDATA[wx2421b1c4370ec43b]]></appid><attach><![CDATA[支付测试]]></attach><bank_type><![CDATA[CFT]]></bank_type><fee_type><![CDATA[CNY]]></fee_type><is_subscribe><![CDATA[Y]]></is_subscribe><mch_id><![CDATA[10000100]]></mch_id><nonce_str><![CDATA[5d2b6c2a8db53831f7eda20af46e531c]]></nonce_str><openid><![CDATA[oUpF8uMEb4qRXf22hE3X68TekukE]]></openid><out_trade_no><![CDATA[1409811653]]></out_trade_no><result_code><![CDATA[SUCCESS]]></result_code><return_code><![CDATA[SUCCESS]]></return_code><sign><![CDATA[B552ED6B279343CB493C5DD0D78AB241]]></sign><sub_mch_id><![CDATA[10000100]]></sub_mch_id><time_end><![CDATA[20140903131540]]></time_end><total_fee>1</total_fee><coupon_fee><![CDATA[10]]></coupon_fee><coupon_count><![CDATA[1]]></coupon_count><coupon_type><![CDATA[CASH]]></coupon_type><coupon_id><![CDATA[10000]]></coupon_id><coupon_fee><![CDATA[100]]></coupon_fee><trade_type><![CDATA[JSAPI]]></trade_type><transaction_id><![CDATA[1004400740201409030005092168]]></transaction_id></xml>";

            /*DataSet ds = new DataSet();
            StringReader stram = new StringReader(result);
            XmlTextReader reader = new XmlTextReader(stram);
            ds.ReadXml(reader);

            var amount = ds.Tables[0].Rows[0]["total_fee"].ToString();
            var recordId = ds.Tables[0].Rows[0]["out_trade_no"].ToString();
            var timeEnd = ds.Tables[0].Rows[0]["time_end"].ToString();
            var resultCode = ds.Tables[0].Rows[0]["result_code"].ToString();

            var record = await _balanceRecordRepository.GetAsync(Guid.Parse(recordId));
            record.WechatPayId = ds.Tables[0].Rows[0]["transaction_id"].ToString();
            record.PaymentCompleteTime = new DateTime(Int32.Parse(timeEnd.Substring(0, 4)), Int32.Parse(timeEnd.Substring(4, 2)), Int32.Parse(timeEnd.Substring(6, 2)),
                Int32.Parse(timeEnd.Substring(8, 2)), Int32.Parse(timeEnd.Substring(10, 2)), Int32.Parse(timeEnd.Substring(12, 2)));
            record.IsPaidSuccessfully = resultCode == "SUCCESS";
            await _balanceRecordRepository.UpdateAsync(record);


            var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
            if (balance == null)
            {
                await _balanceRepository.InsertAsync(new UserBalance
                {
                    TotalBalance = double.Parse(amount),
                    FrozenBalance = 0,
                    UserId = record.UserId
                });
            }
            else
            {
                balance.TotalBalance += double.Parse(amount);
                await _balanceRepository.UpdateAsync(balance);
            }#1#

            return "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";
        }*/

        [UnitOfWork]
        public async Task Withdraw(WithdrawInputDto input)
        {
            if (AbpSession.UserId.HasValue)
            {
                var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
                if (balance == null)
                {
                    throw new UserFriendlyException("没有足够的余额可提现");
                }

                if (balance.TotalBalance - balance.FrozenBalance < input.Amount)
                {
                    throw new UserFriendlyException("没有足够的余额可提现");
                }

                balance.TotalBalance -= input.Amount;
                await _balanceRepository.UpdateAsync(balance);

                await _balanceRecordRepository.InsertAsync(new UserBalanceRecord
                {
                    Amount = -input.Amount,
                    RecordTime = DateTime.Now,
                    Remarks = "提现",
                    UserId = AbpSession.UserId.Value
                });
            }
            else
            {
                throw new UserFriendlyException("用户未登录");
            }
        }

        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum) { str += "0123456789"; }
            if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        public string GetSign(Dictionary<string, string> dict, string key)
        {
            dict = dict.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            var sign = dict.Aggregate("", (current, d) => current + d.Key + "=" + d.Value + "&");
            sign += "key=" + key;
            using (var md5 = MD5.Create())
            {
                sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sign))).Replace("-", "");
                return sign;
            }
        }
    }
}