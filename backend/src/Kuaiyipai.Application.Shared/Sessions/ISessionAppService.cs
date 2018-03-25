using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.Sessions.Dto;

namespace Kuaiyipai.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
