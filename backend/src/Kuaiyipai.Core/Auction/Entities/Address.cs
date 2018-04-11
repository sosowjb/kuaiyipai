using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_Addresses")]
    public class Address : Entity<Guid>
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
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}