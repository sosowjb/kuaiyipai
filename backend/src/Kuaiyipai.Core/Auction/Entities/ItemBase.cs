using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.Auction.Entities
{
    public abstract class ItemBase : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 商品编号：Pillar Code + Category Code + timestamp
        /// 001+002+636582163564955981
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        public int PillarId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 起步价
        /// </summary>
        [Required]
        public double StartPrice { get; set; }

        /// <summary>
        /// 加价幅度
        /// </summary>
        [Required]
        public double StepPrice { get; set; }

        /// <summary>
        /// 封顶价
        /// </summary>
        public double? PriceLimit { get; set; }

        /// <summary>
        /// 开始拍卖时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束拍卖时间
        /// </summary>
        public DateTime Deadline { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int BiddingCount { get; set; }

        public double HighestBiddingPrice { get; set; }

        public string InvitationCode { get; set; }

        public Guid? SpecialActivityId { get; set; }
    }
}