using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Address.Dto
{
    public class GetAddressesOutputDto : EntityDto<Guid>
    {
        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string Receiver { get; set; }

        public string ContactPhoneNumber { get; set; }

        public bool IsDefault { get; set; }
    }
}