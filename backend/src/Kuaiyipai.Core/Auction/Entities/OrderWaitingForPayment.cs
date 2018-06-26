﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForPayment")]
    public class OrderWaitingForPayment : OrderBase
    {
        public DateTime? PaidTime { get; set; }
    }
}