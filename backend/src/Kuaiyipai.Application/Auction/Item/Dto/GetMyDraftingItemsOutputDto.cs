using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class GetMyDraftingItemsOutputDto : EntityDto<Guid>
    {
        public string Pillar { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double StartPrice { get; set; }

        public double StepPrice { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime Deadline { get; set; }
    }
}