using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class GetAuctionItemsOutputDto : EntityDto<Guid>
    {
        public string Pillar { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverPic { get; set; }

        /// <summary>
        /// 封面宽度
        /// </summary>
        public int CoverPicWidth { get; set; }

        /// <summary>
        /// 封面高度
        /// </summary>
        public int CoverPicHeight { get; set; }

        public double StartPrice { get; set; }

        public double StepPrice { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime Deadline { get; set; }
    }
}
