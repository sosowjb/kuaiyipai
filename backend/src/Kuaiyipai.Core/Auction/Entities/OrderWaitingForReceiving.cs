﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Orders_WaitingForReceiving")]
    public class OrderWaitingForReceiving : OrderBase
    {
        public DateTime? ReceivedTime { get; set; }
    }
}