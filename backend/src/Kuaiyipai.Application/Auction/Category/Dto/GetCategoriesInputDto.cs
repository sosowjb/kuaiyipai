using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Category.Dto
{
    public class GetCategoriesInputDto : PagedAndSortedResultRequestDto
    {
        public int? PillarId { get; set; }
    }
}