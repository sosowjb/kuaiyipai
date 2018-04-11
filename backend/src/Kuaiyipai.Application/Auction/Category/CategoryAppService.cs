using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Kuaiyipai.Auction.Category.Dto;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.Category
{
    public class CategoryAppService : KuaiyipaiAppServiceBase, ICategoryAppService
    {
        private readonly IPillarAndCategoryManager _pillarAndCategoryManager;
        private readonly IRepository<Entities.Category> _categoryRepository;

        public CategoryAppService(IPillarAndCategoryManager pillarAndCategoryManager, IRepository<Entities.Category> categoryRepository)
        {
            _pillarAndCategoryManager = pillarAndCategoryManager;
            _categoryRepository = categoryRepository;
        }

        public async Task<int> CreateCategory(CreateCategoryInputDto input)
        {
            return await _pillarAndCategoryManager.CreateCategory(input.Name, input.PillarId);
        }

        public async Task UpdateCategory(UpdateCategoryInputDto input)
        {
            await _pillarAndCategoryManager.UpdateCategory(input.PillarId, input.Name);
        }

        public async Task DeleteCategory(EntityDto input)
        {
            await _pillarAndCategoryManager.DeleteCategory(input.Id);
        }

        public async Task<PagedResultDto<GetCategoriesOutputDto>> GetCategories(GetCategoriesInputDto input)
        {
            var query = _categoryRepository.GetAll();

            if (input.PillarId.HasValue)
            {
                query = query.Where(c => c.PillarId == input.PillarId.Value);
            }

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(category => new GetCategoriesOutputDto
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                PillarId = category.PillarId
            }).ToListAsync();

            return new PagedResultDto<GetCategoriesOutputDto>(count, list);
        }
    }
}