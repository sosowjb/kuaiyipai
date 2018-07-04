using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Authorization
{
    public class UserPhoneAppService : ApplicationService, IUserPhoneAppService
    {
        private readonly IRepository<User, long> _userRepository;

        public UserPhoneAppService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task BindPhone(BindPhoneInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            // temp
            if (input.Captcha != "000000")
            {
                throw new UserFriendlyException("验证码不正确");
            }

            var user = await _userRepository.FirstOrDefaultAsync(AbpSession.UserId.Value);
            user.PhoneNumber = input.Phone;

            await _userRepository.UpdateAsync(user);
        }
    }
}