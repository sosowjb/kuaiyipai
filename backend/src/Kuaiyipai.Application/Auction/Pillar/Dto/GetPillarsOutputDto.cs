using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Pillar.Dto
{
    public class GetPillarsOutputDto : EntityDto
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}