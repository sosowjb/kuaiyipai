using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Auction.Category.Dto
{
    public class CreateCategoryInputDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int PillarId { get; set; }
    }
}