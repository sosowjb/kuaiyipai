﻿using System;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class PayInputDto
    {
        public Guid OrderId { get; set; }

        public Guid AddressId { get; set; }
    }
}