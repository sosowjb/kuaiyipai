using System.Threading.Tasks;
using Abp.Application.Services;

namespace Kuaiyipai.Authorization
{
    public interface IUserInfoAppService : IApplicationService
    {
        Task<UserInfoDto> GetUserInfo();
    }
}