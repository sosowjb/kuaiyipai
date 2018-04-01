using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Categories")]
    public class Category : Entity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(3)]
        public string Code { get; set; }

        [Required]
        public int PillarId { get; set; }

        [ForeignKey("PillarId")]
        public Pillar Pillar { get; set; }
    }
}