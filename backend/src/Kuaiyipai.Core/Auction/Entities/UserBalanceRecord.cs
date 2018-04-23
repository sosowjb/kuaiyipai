using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_BalanceRecords")]
    public class UserBalanceRecord : Entity<Guid>
    {
        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime RecordTime { get; set; }

        public string Remarks { get; set; }
    }
}