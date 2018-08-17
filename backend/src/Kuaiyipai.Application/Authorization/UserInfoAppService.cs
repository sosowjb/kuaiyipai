using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Authorization
{
    public class UserInfoAppService : ApplicationService, IUserInfoAppService
    {
        private readonly IRepository<User, long> _userRepository;

        public UserInfoAppService(IRepository<User, long> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserInfoDto> GetUserInfo()
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

            return new UserInfoDto
            {
                Id = user.Id,
                NickName = user.NickName,
                AvatarUrl = user.AvatarLink,
                Phone = user.PhoneNumber
            };
        }
    }
}