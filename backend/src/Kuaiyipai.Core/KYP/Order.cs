using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.KYP
{
    [Table("KYP_Orders")]
    public class Order : Entity<Guid>
    {
        public DateTime OrderTime { get; set; }

        public OrderStatus Status { get; set; }

        public Guid ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public long BuyerId { get; set; }

        public long SellerId { get; set; }

        public double AmountPrice { get; set; }

        public long AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }
    }
}