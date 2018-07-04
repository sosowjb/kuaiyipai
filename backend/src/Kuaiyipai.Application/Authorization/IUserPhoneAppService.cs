using System.Threading.Tasks;
using Abp.Application.Services;

namespace Kuaiyipai.Authorization
{
    public interface IUserPhoneAppService : IApplicationService
    {
        Task BindPhone(BindPhoneInputDto input);
    }
}