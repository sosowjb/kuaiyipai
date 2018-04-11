using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Pillar.Dto
{
    public class UpdatePillarInputDto : EntityDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}