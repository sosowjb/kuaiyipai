using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Orders")]
    public class Order : CreationAuditedEntity<long>, ISoftDelete
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

        /// <summary>
        /// 快递单号
        /// </summary>
        public string DeliveryId { get; set; }

        [Required]
        public long AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public bool IsDeleted { get; set; }
    }
}