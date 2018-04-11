using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForEvaluating")]
    public class OrderWaitingForEvaluating : OrderBase
    {
        public bool Evaluated { get; set; }

        public DateTime? EvaluatedTime { get; set; }

        [Range(1, 5)]
        public int? Level { get; set; }
    }
}