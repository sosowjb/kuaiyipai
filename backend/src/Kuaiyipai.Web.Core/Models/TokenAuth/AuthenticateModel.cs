﻿using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace Kuaiyipai.Web.Models.TokenAuth
{
    public class AuthenticateModel
    {
        //[Required]
        //[MaxLength(AbpUserBase.MaxEmailAddressLength)]
        //public string UserNameOrEmailAddress { get; set; }

        //[Required]
        //[MaxLength(AbpUserBase.MaxPlainPasswordLength)]
        //[DisableAuditing]
        //public string Password { get; set; }

        //public string TwoFactorVerificationCode { get; set; }

        //public bool RememberClient { get; set; }

        //public string TwoFactorRememberClientToken { get; set; }

        //public bool? SingleSignIn { get; set; }

        //public string ReturnUrl { get; set; }
        
        public string NickName { get; set; }
        
        public string Code { get; set; }
        
        public string AvatarLink { get; set; }
    }
}