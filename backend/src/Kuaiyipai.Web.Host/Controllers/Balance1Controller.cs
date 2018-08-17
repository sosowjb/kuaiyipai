using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Kuaiyipai.Web.Controllers
{
    public class Balance1Controller : ControllerBase
    {
        [Route("api/Balance1/ComplateCharge")]
        [HttpPost]
        [DisableAuditing]
        public ContentResult ComplateCharge()
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
            }*/

            return new ContentResult() { Content = "<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>" };
        }
    }
}
