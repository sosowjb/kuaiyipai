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

        /// <summary>
        /// 微信支付定单号
        /// </summary>
        public string WechatPayId { get; set; }

        /// <summary>
        /// 是否成功支付
        /// </summary>
        public bool? IsPaidSuccessfully { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime PaymentCompleteTime { get; set; }
    }
}