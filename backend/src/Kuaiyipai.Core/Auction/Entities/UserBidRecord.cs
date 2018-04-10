using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_BidRecords")]
    public class UserBidRecord : CreationAuditedEntity<Guid>
    {
        [Required]
        public Guid ItemId { get; set; }

        [ForeignKey("ItemId")]
        public ItemAuctioning Item { get; set; }

        [Required]
        public double Price { get; set; }
    }
}