using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}