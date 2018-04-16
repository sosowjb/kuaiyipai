using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForReceiving")]
    public class OrderWaitingForReceiving : OrderBase
    {
        [Required]
        public DateTime ReceivedTime { get; set; }
    }
}