using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.Editions.Dto;
using Kuaiyipai.MultiTenancy.Dto;

namespace Kuaiyipai.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}