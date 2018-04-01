using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Bids")]
    public class Bid : CreationAuditedEntity<long>
    {
        [Required]
        public long ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        [Required]
        public double Price { get; set; }
    }
}