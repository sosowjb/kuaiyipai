using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Areas")]
    public class Area : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public int Level { get; set; }
    }
}