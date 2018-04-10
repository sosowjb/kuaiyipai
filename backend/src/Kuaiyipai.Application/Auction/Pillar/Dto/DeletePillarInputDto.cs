using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Pillar.Dto
{
    public class DeletePillarInputDto : EntityDto
    {
        public bool Recursive { get; set; }
    }
}