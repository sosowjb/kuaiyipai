using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Auction.Address.Dto
{
    public class CreateAddressInputDto
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

        public bool IsDefault { get; set; }
    }
}