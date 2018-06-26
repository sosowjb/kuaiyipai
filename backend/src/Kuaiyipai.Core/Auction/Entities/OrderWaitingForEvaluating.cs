﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForEvaluating")]
    public class OrderWaitingForEvaluating : OrderBase
    {
        public DateTime? EvaluatedTime { get; set; }
    }
}