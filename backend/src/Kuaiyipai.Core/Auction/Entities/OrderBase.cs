using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.Auction.Entities
{
    public abstract class OrderBase : Entity<Guid>
    {
        /// <summary>
        /// 订单编号
        /// 20180928190807123+*****
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        public long BuyerId { get; set; }

        [Required]
        public long SellerId { get; set; }

        [Required]
        public Guid AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string DeliveryId { get; set; }

        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 商品最终价格
        /// </summary>
        public double ItemPriceAmount { get; set; }

        /// <summary>
        /// 快递费
        /// </summary>
        public double ExpressCostAmount { get; set; }
    }
}