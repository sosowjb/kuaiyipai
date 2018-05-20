using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kuaiyipai.Auction.Balance
{
    public class BalanceAppService : KuaiyipaiAppServiceBase, IBalanceAppService
    {
        private const string LoginApi = "https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code";
        private const string OrderApi = "https://api.mch.weixin.qq.com/pay/unifiedorder";

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
            var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
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
        public async Task Charge(ChargeInputDto input)
        {
            var appId = _appConfiguration["WeChat:AppId"];
            var appSecret = _appConfiguration["WeChat:AppSecret"];
            var merchantId = _appConfiguration["WeChat:PayMerchantId"];
            var paymentApiKey = _appConfiguration["WeChat:PaymentApiKey"];
            var remoteIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var notifyUrl = new Uri(new Uri(_appConfiguration["App:ServerRootAddress"]), "/api/services/app/Balance/CompleteCharge").ToString();

            // 获取OpenID
            var api = LoginApi
                .Replace("APPID", appId)
                .Replace("SECRET", appSecret)
                .Replace("JSCODE", input.LoginCode);
            var s = await HttpHelper.Get(api, string.Empty);
            var jo = (JObject)JsonConvert.DeserializeObject(s);
            string openId;
            try
            {
                openId = jo["openid"].ToString();
            }
            catch
            {
                throw new UserFriendlyException("获取OpenID失败，可能是Code已过期");
            }

            // 修改账户余额
            Guid recordId;
            if (AbpSession.UserId.HasValue)
            {
                var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId);
                if (balance == null)
                {
                    await _balanceRepository.InsertAsync(new UserBalance
                    {
                        TotalBalance = input.Amount,
                        FrozenBalance = 0,
                        UserId = AbpSession.UserId.Value
                    });
                }
                else
                {
                    balance.TotalBalance += input.Amount;
                    await _balanceRepository.UpdateAsync(balance);
                }

                recordId = await _balanceRecordRepository.InsertAndGetIdAsync(new UserBalanceRecord
                {
                    Amount = input.Amount,
                    RecordTime = DateTime.Now,
                    Remarks = "充值",
                    UserId = AbpSession.UserId.Value
                });
            }
            else
            {
                throw new UserFriendlyException("用户未登录");
            }

            // 调用统一下单接口
            var paramsDict = new Dictionary<string, string>
            {
                {"appid", appId},
                {"mch_id", merchantId},
                {"nonce_str", GetRandomString(32, true, false, true, false, "")},
                {"body", "快微拍-余额充值"},
                {"out_trade_no", recordId.ToString().Replace("-", "").ToUpper()},
                {"total_fee", input.Amount.ToString(CultureInfo.InvariantCulture)},
                {"spbill_create_ip", "192.168.0.1"},
                {"notify_url", notifyUrl},
                {"trade_type", "JSAPI"},
                {"openid", openId}
            };
            paramsDict.Add("sign", GetSign(paramsDict, paymentApiKey));
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var d in paramsDict)
            {
                sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
            }
            sb.Append("</xml>");

            var result = await HttpHelper.Post(OrderApi, sb.ToString());
        }

        public Task CompleteCharge()
        {
            return Task.CompletedTask;
        }

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