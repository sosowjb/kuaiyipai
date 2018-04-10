using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Category.Dto
{
    public class UpdateCategoryInputDto : EntityDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int PillarId { get; set; }
    }
}