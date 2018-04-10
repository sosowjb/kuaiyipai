using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Category.Dto;

namespace Kuaiyipai.Auction.Category
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<int> CreateCategory(CreateCategoryInputDto input);

        Task UpdateCategory(UpdateCategoryInputDto input);

        Task DeleteCategory(EntityDto input);

        Task<PagedResultDto<GetCategoriesOutputDto>> GetCategories(GetCategoriesInputDto input);
    }
}