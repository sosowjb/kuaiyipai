using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForEvaluating")]
    public class OrderWaitingForEvaluating : OrderBase
    {
        [Required]
        public DateTime EvaluatedTime { get; set; }
    }
}