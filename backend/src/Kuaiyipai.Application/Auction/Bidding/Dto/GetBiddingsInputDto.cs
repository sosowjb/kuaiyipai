using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Bidding.Dto
{
    public class GetBiddingsInputDto : PagedAndSortedResultRequestDto
    {
        public Guid ItemId { get; set; }
    }
}