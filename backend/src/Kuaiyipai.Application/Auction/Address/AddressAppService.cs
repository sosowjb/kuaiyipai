using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Address.Dto;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.Address
{
    public class AddressAppService : KuaiyipaiAppServiceBase, IAddressAppService
    {
        private readonly IRepository<Entities.Address, Guid> _addressRepository;

        public AddressAppService(IRepository<Entities.Address, Guid> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [UnitOfWork]
        public async Task<Guid> CreateAddress(CreateAddressInputDto input)
        {
            if (AbpSession.UserId != null)
            {
                if (input.IsDefault)
                {
                    var addresses = await _addressRepository.GetAll().Where(a => a.CreatorUserId == AbpSession.UserId).ToListAsync();
                    foreach (var address in addresses)
                    {
                        address.IsDefault = false;
                        await _addressRepository.UpdateAsync(address);
                    }
                }

                return await _addressRepository.InsertAndGetIdAsync(new Entities.Address
                {
                    ProvinceId = input.ProvinceId,
                    CityId = input.CityId,
                    DistrictId = input.DistrictId,
                    Street = input.Street,
                    Receiver = input.Receiver,
                    ContactPhoneNumber = input.ContactPhoneNumber,
                    IsDefault = input.IsDefault
                });
            }
            throw new UserFriendlyException("用户ID不存在");
        }

        public async Task UpdateAddress(UpdateAddressInputDto input)
        {
            var address = await _addressRepository.GetAsync(input.Id);
            address.ProvinceId = input.ProvinceId;
            address.CityId = input.CityId;
            address.DistrictId = input.DistrictId;
            address.Street = input.Street;
            address.Receiver = input.Receiver;
            address.ContactPhoneNumber = input.ContactPhoneNumber;
            await _addressRepository.UpdateAsync(address);
        }

        [UnitOfWork]
        public async Task SetDefault(EntityDto<Guid> input)
        {
            var addresses = await _addressRepository.GetAll().Where(a => a.CreatorUserId == AbpSession.UserId).ToListAsync();
            foreach (var address in addresses)
            {
                address.IsDefault = address.Id == input.Id;
                await _addressRepository.UpdateAsync(address);
            }
        }

        public async Task DeleteAddress(EntityDto<Guid> input)
        {
            var address = await _addressRepository.FirstOrDefaultAsync(input.Id);
            if (address == null)
            {
                throw new UserFriendlyException("地址不存在");
            }

            if (address.IsDefault)
            {
                throw new UserFriendlyException("不能删除默认地址");
            }

            await _addressRepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetAddressesOutputDto>> GetAddress(GetAddressesInputDto input)
        {


            var query = _addressRepository.GetAll().Where(a => a.CreatorUserId == AbpSession.UserId);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            if (!input.Id.IsNullOrEmpty())
            {
                Guid guid = new Guid(input.Id);
                query = query.Where(a => a.Id == guid);
            }
            var count = await query.CountAsync();

            var list = await query.PageBy(input).Select(address => new GetAddressesOutputDto
            {
                Id = address.Id,
                Street = address.Street,
                IsDefault = address.IsDefault,
                Province = address.ProvinceId.ToString(),
                City = address.CityId.ToString(),
                District = address.DistrictId.ToString(),
                Receiver = address.Receiver,
                ContactPhoneNumber = address.ContactPhoneNumber
            }).ToListAsync();

            return new PagedResultDto<GetAddressesOutputDto>(count, list);
        }

        public async Task<GetAddressesOutputDto> GetDefaultAddress()
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var address = await _addressRepository.GetAll()
                .FirstOrDefaultAsync(a => a.CreatorUserId == AbpSession.UserId.Value && a.IsDefault);

            if (address == null)
            {
                address = await _addressRepository.GetAll().FirstOrDefaultAsync(a => a.CreatorUserId == AbpSession.UserId.Value);
                if (address == null)
                {
                    throw new UserFriendlyException("用户没有设置任何地址");
                }
            }

            return new GetAddressesOutputDto
            {
                Id = address.Id,
                Province = address.ProvinceId.ToString(),
                City = address.CityId.ToString(),
                District = address.DistrictId.ToString(),
                Street = address.Street,
                Receiver = address.Receiver,
                ContactPhoneNumber = address.ContactPhoneNumber,
                IsDefault = address.IsDefault
            };
        }
    }
}