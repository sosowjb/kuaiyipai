using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Auction.Pillar.Dto
{
    public class CreatePillarInputDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}