using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForSending")]
    public class OrderWaitingForSending : OrderBase
    {
        public bool Sent { get; set; }

        public DateTime? SentTime { get; set; }
    }
}