using System;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class UpdateOrderAddressInputDto
    {
        public Guid OrderId { get; set; }

        public Guid AddressId { get; set; }
    }
}