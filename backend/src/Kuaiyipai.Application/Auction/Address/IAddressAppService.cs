using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Address.Dto;

namespace Kuaiyipai.Auction.Address
{
    public interface IAddressAppService : IApplicationService
    {
        Task<Guid> CreateAddress(CreateAddressInputDto input);

        Task UpdateAddress(UpdateAddressInputDto input);

        Task SetDefault(EntityDto<Guid> input);

        Task DeleteAddress(EntityDto<Guid> input);

        Task<PagedResultDto<GetAddressesOutputDto>> GetAddress(GetAddressesInputDto input);
    }
}