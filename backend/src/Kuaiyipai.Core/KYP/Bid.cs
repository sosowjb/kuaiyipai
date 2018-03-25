using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.KYP
{
    [Table("KYP_Bids")]
    public class Bid : CreationAuditedEntity<Guid>
    {
        public double Price { get; set; }

        public Guid ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public int Order { get; set; }
    }
}