using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.KYP.Entities
{
    [Table("KYP_Addresses")]
    public class Address : Entity<long>
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
        public long AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}