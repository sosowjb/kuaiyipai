using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class GetMyCompletedItemsOutputDto : EntityDto<Guid>
    {
        public string Pillar { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double StartPrice { get; set; }

        public double StepPrice { get; set; }

        public string StartTime { get; set; }

        public string Deadline { get; set; }

        public int BiddingCount { get; set; }

        public double HighestBiddingPrice { get; set; }
    }
}