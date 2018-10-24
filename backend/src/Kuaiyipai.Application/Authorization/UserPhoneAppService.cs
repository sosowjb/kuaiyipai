using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using IdentityServer4.Extensions;
using Kuaiyipai.Authorization.Users;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;

namespace Kuaiyipai.Authorization
{
    public class UserPhoneAppService : ApplicationService, IUserPhoneAppService
    {
        private readonly IRepository<User, long> _userRepository;

        public UserPhoneAppService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RequestCaptcha(RequestCaptchaInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var user = await _userRepository.FirstOrDefaultAsync(AbpSession.UserId.Value);

            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }

            Random rd = new Random();
            int i = rd.Next(0, 999999);

            var captcha = i.ToString().PadLeft(6, '0');
            user.Captcha = captcha;
            await _userRepository.UpdateAsync(user);

            SendSms(input.PhoneNum, captcha);
        }

        public async Task BindPhone(BindPhoneInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            if (input.Captcha.IsNullOrEmpty() || input.Captcha.Length > 6)
            {
                throw new UserFriendlyException("验证码不正确");
            }

            var user = await _userRepository.FirstOrDefaultAsync(u =>
                u.Id == AbpSession.UserId.Value && u.Captcha == input.Captcha);

            if (user == null)
            {
                throw new UserFriendlyException("验证码不正确");
            }

            user.PhoneNumber = input.Phone;
            user.Captcha = "";

            await _userRepository.UpdateAsync(user);
        }



        //产品名称:云通信短信API产品,开发者无需替换
        const String product = "Dysmsapi";
        //产品域名,开发者无需替换
        const String domain = "dysmsapi.aliyuncs.com";

        // TODO 此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
        const String accessKeyId = "LTAIaEP1GbiMUXa5";
        const String accessKeySecret = "KluRss5a0t3EkW7bdXENw8tbPgEEMg";
        public SendSmsResponse SendSms(string phone, string captcha)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SendSmsResponse response = null;
            try
            {

                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phone;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "快微拍";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = "SMS_148612333";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"code\":\"" + captcha + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "1";
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);

            }
            catch (ServerException e)
            {
                Console.WriteLine(e.ErrorCode);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e.ErrorCode);
            }
            return response;

        }
    }
}