using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class GetWaitingForReceivingOrdersOutputDto : EntityDto<Guid>
    {
        public string Code { get; set; }

        public long BuyerId { get; set; }

        public string BuyerName { get; set; }

        public long SellerId { get; set; }

        public string SellerName { get; set; }

        public string ContactPhoneNumber { get; set; }

        public Guid AddressId { get; set; }

        public string Address { get; set; }

        public string DeliveryId { get; set; }

        public DateTime OrderTime { get; set; }

        public double Amount { get; set; }

        public double ItemPriceAmount { get; set; }

        public double ExpressCostAmount { get; set; }
    }
}