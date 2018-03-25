using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.KYP
{
    [Table("KYP_Items")]
    public class Item : FullAuditedEntity<Guid>
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double StartPrice { get; set; }

        public double Step { get; set; }

        public DateTime Deadline { get; set; }

        public ItemStatus Status { get; set; }
    }
}