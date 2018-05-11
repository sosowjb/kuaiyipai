using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Kuaiyipai.Authorization;
using Kuaiyipai.Authorization.Impersonation;
using Kuaiyipai.Authorization.Users;
using Kuaiyipai.Web.Authentication.JwtBearer;
using Kuaiyipai.Web.Models.TokenAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kuaiyipai.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : KuaiyipaiControllerBase
    {
        private const string WeChatApi = "https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code";

        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IdentityOptions _identityOptions;
        private readonly IRepository<User, long> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            TokenAuthConfiguration configuration,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            IOptions<IdentityOptions> identityOptions,
            IRepository<User, long> userRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _configuration = configuration;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _identityOptions = identityOptions.Value;
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            // get open id
            var api = WeChatApi
                .Replace("APPID", KuaiyipaiWebCoreModule.AppId)
                .Replace("SECRET", KuaiyipaiWebCoreModule.AppSecret)
                .Replace("JSCODE", model.Code);
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

            // login
            var loginResult = await _logInManager.LoginAsync(openId, openId, GetTenancyNameOrNull());
            if (loginResult.Result == AbpLoginResultType.Success)
            {
                await UpdateUser(loginResult.User, model.Name, model.AvatarLink);

                //get access code
                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = loginResult.User.Id
                };
            }

            // login failed then register
            await CreateUser(model.Name, openId, model.AvatarLink);
            return await ReAuthenticate(model.Name, openId, model.AvatarLink);
        }

        [UnitOfWork(IsDisabled = true)]
        private async Task<AuthenticateResultModel> ReAuthenticate(string name, string openId, string avatarLink)
        {
            var loginResult = await _logInManager.LoginAsync(openId, openId, GetTenancyNameOrNull());
            if (loginResult.Result == AbpLoginResultType.Success)
            {
                //get access code
                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = loginResult.User.Id
                };
            }

            // login failed then register
            await CreateUser(name, openId, avatarLink);
            return await ReAuthenticate(name, openId, avatarLink);
        }

        private async Task CreateUser(string name, string openId, string avatarLink)
        {
            var user = new User
            {
                TenantId = AbpSession.TenantId,
                Name = " ",
                Surname = " ",
                EmailAddress = openId + "@kuaiyipai.net",
                IsActive = true,
                UserName = openId,
                IsEmailConfirmed = true,
                Roles = new List<UserRole>(),
                NickName = name,
                AvatarLink = avatarLink
            };
            user.SetNormalizedNames();
            user.Password = _passwordHasher.HashPassword(user, openId);
            await _userRepository.InsertAsync(user);
        }

        private async Task UpdateUser(User user, string name, string avatarLink)
        {
            user.Name = name;
            user.Surname = name;
            user.AvatarLink = avatarLink;
            await _userRepository.UpdateAsync(user);
        }

        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var accessToken = CreateAccessToken(CreateJwtClaims(result.Identity));

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }

        [HttpPost]
        public async Task<SwitchedAccountAuthenticateResultModel> LinkedAccountAuthenticate(string switchAccountToken)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(switchAccountToken);
            var accessToken = CreateAccessToken(CreateJwtClaims(result.Identity));

            return new SwitchedAccountAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        private List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);

            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }
    }
}
