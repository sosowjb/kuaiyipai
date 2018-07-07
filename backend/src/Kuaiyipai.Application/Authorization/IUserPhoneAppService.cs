using System.Threading.Tasks;
using Abp.Application.Services;

namespace Kuaiyipai.Authorization
{
    public interface IUserPhoneAppService : IApplicationService
    {
        Task RequestCaptcha(RequestCaptchaInputDto input);

        Task BindPhone(BindPhoneInputDto input);
    }
}