using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Addresses")]
    public class Address : CreationAuditedEntity<Guid>
    {
        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Required]
        [StringLength(500)]
        public string Street { get; set; }

        public bool IsDefault { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Receiver { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ContactPhoneNumber { get; set; }
    }
}