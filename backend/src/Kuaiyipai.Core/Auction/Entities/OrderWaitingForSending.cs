using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForSending")]
    public class OrderWaitingForSending : OrderBase
    {
        public DateTime? SentTime { get; set; }
    }
}