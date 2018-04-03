using System.Threading.Tasks;
using Kuaiyipai.KYP.Pillar.Dto;

namespace Kuaiyipai.KYP.Pillar
{
    public class PillarAppService : KuaiyipaiAppServiceBase, IPillarAppService
    {
        private readonly IPillarAndCategoryManager _pillarAndCategoryManager;

        public PillarAppService(IPillarAndCategoryManager pillarAndCategoryManager)
        {
            _pillarAndCategoryManager = pillarAndCategoryManager;
        }

        public async Task CreatePillar(CreatePillarInputDto input)
        {
            await _pillarAndCategoryManager.CreatePillar(new Entities.Pillar
            {
                Name = input.Name
            });
        }
    }
}