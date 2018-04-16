using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Address.Dto
{
    public class UpdateAddressInputDto : EntityDto<Guid>
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

        [Required]
        [StringLength(50)]
        public string Receiver { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactPhoneNumber { get; set; }
    }
}