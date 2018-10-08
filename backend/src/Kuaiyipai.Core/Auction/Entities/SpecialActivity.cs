using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_SpecialActivities")]
    public class SpecialActivity : CreationAuditedEntity<Guid>
    {
        public string InvitationCode { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CoverUrl { get; set; }
    }
}