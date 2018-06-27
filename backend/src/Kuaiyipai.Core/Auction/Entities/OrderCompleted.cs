using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_Completed")]
    public class OrderCompleted : OrderBase
    {
        [Range(1, 5)]
        public int? EvaluationLevel { get; set; }

        public string EvaluationContent { get; set; }

        [Required]
        public DateTime CompletedTime { get; set; }
    }
}