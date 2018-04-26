using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.Auction.Entities
{
    [Table("ACU_Categories")]
    public class Category : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(5)]
        public string Code { get; set; }

        [Required]
        public int PillarId { get; set; }

        [ForeignKey("PillarId")]
        public Pillar Pillar { get; set; }
    }
}