using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class GetTerminatedItemOutputDto : EntityDto<Guid>
    {
        public string Code { get; set; }
        
        public int PillarId { get; set; }
        
        public int CategoryId { get; set; }
        
        public double StartPrice { get; set; }
        
        public double StepPrice { get; set; }
        
        public double? PriceLimit { get; set; }
        
        public DateTime? StartTime { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }

        public int BiddingCount { get; set; }

        public double HighestBiddingPrice { get; set; }
    }
}