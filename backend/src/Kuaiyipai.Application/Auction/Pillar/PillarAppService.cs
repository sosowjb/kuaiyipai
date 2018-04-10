using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Pillar.Dto;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.Pillar
{
    public class PillarAppService : KuaiyipaiAppServiceBase, IPillarAppService
    {
        private readonly IPillarAndCategoryManager _pillarAndCategoryManager;
        private readonly IRepository<Entities.Pillar> _pillarRepository;

        public PillarAppService(IPillarAndCategoryManager pillarAndCategoryManager, IRepository<Entities.Pillar> pillarRepository)
        {
            _pillarAndCategoryManager = pillarAndCategoryManager;
            _pillarRepository = pillarRepository;
        }
        
        public async Task<int> CreatePillar(CreatePillarInputDto input)
        {
            return await _pillarAndCategoryManager.CreatePillar(input.Name);
        }
        
        public async Task UpdatePillar(UpdatePillarInputDto input)
        {
            await _pillarAndCategoryManager.UpdatePillar(input.Id, input.Name);
        }
        
        public async Task DeletePillar(DeletePillarInputDto input)
        {
            await _pillarAndCategoryManager.DeletePillar(input.Id, input.Recursive);
        }
        
        public async Task<PagedResultDto<GetPillarsOutputDto>> GetPillars(GetPillarsInputDto input)
        {
            var query = _pillarRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(pillar => new GetPillarsOutputDto
            {
                Id = pillar.Id,
                Code = pillar.Code,
                Name = pillar.Name
            }).ToListAsync();

            return new PagedResultDto<GetPillarsOutputDto>(count, list);
        }
    }
}