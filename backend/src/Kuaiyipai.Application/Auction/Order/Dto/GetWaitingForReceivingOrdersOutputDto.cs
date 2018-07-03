using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class GetWaitingForReceivingOrdersOutputDto : EntityDto<Guid>
    {
        public string Code { get; set; }

        public long BuyerId { get; set; }

        public long SellerId { get; set; }

        public Guid AddressId { get; set; }

        public string DeliveryId { get; set; }

        public DateTime OrderTime { get; set; }

        public double Amount { get; set; }

        public double ItemPriceAmount { get; set; }

        public double ExpressCostAmount { get; set; }

        public string ItemTitle { get; set; }

        public string ItemPicUrl { get; set; }
    }
}