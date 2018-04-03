using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.KYP.Pillar.Dto;

namespace Kuaiyipai.KYP.Pillar
{
    public interface IPillarAppService : IApplicationService
    {
        Task CreatePillar(CreatePillarInputDto input);
    }
}