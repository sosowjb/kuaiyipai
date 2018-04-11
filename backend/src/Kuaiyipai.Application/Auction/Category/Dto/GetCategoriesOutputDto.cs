using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Category.Dto
{
    public class GetCategoriesOutputDto : EntityDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int PillarId { get; set; }
    }
}