using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class OrderDetailDto : EntityDto<Guid>
    {
        public int OrderStatus { get; set; }

        public DateTime OrderTime { get; set; }

        public string Address { get; set; }

        public string GoodsName { get; set; }

        public double Price { get; set; }

        public DateTime? PaidTime { get; set; }

        public string SellerTel { get; set; }

        public string AuctionNum { get; set; }

        public string GoodsPicture { get; set; }

        public string BuyerName { get; set; }

        public string BuyerTel { get; set; }

        public string DeliveryType { get; set; }

        public string DeliveryId { get; set; }
    }
}