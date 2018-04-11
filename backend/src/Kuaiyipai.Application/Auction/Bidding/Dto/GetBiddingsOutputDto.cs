using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Bidding.Dto
{
    public class GetBiddingsOutputDto : EntityDto<Guid>
    {
        public Guid ItemId { get; set; }

        public DateTime BidTime { get; set; }

        public double Price { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }
    }
}