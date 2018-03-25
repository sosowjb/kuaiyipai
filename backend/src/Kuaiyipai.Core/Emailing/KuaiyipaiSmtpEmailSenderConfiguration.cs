using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace Kuaiyipai.Emailing
{
    public class KuaiyipaiSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public KuaiyipaiSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}