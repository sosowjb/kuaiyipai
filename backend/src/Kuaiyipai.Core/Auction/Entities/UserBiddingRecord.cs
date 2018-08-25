using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_BiddingRecords")]
    public class UserBiddingRecord : CreationAuditedEntity<Guid>
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public double Price { get; set; }
    }
}