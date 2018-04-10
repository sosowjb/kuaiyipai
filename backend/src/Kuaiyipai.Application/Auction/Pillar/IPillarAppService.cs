using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Pillar.Dto;

namespace Kuaiyipai.Auction.Pillar
{
    public interface IPillarAppService : IApplicationService
    {
        Task<int> CreatePillar(CreatePillarInputDto input);

        Task UpdatePillar(UpdatePillarInputDto input);

        Task DeletePillar(DeletePillarInputDto input);

        Task<PagedResultDto<GetPillarsOutputDto>> GetPillars(GetPillarsInputDto input);
    }
}