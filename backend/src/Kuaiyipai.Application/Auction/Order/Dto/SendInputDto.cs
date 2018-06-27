using System;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class SendInputDto
    {
        public Guid OrderId { get; set; }

        public string DeliveryId { get; set; }
    }
}