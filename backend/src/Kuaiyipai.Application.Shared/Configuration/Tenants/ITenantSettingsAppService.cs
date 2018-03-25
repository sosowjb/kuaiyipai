using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.Configuration.Tenants.Dto;

namespace Kuaiyipai.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
