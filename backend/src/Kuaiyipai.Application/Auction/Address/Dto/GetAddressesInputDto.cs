using Abp.Application.Services.Dto;
using System;

namespace Kuaiyipai.Auction.Address.Dto
{
    public class GetAddressesInputDto : PagedAndSortedResultRequestDto
    {
        public string Id { get; set; }
    }
}