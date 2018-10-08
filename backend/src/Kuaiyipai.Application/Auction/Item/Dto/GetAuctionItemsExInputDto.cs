using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class GetAuctionItemsExInputDto : PagedAndSortedResultRequestDto
    {
        public int? PillarId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? PublishTimeStart { get; set; }

        public DateTime? PublishTimeEnd { get; set; }

        public DateTime? DeadlineStart { get; set; }

        public DateTime? DeadlineEnd { get; set; }

        public double? PriceStart { get; set; }

        public double? PriceEnd { get; set; }

        public string Title { get; set; }

        public Guid? SpecialActivityId { get; set; }
    }
}