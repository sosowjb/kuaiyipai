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
using Kuaiyipai.Auction.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.Address
{
    public class AddressAppService : KuaiyipaiAppServiceBase, IAddressAppService
    {
        private readonly IRepository<Entities.Address, Guid> _addressRepository;
        private readonly IRepository<Area> _areaRepository;

        public AddressAppService(IRepository<Entities.Address, Guid> addressRepository, IRepository<Area> areaRepository)
        {
            _addressRepository = addressRepository;
            _areaRepository = areaRepository;
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

            var count = await query.CountAsync();

            var provinceQuery = _areaRepository.GetAll().Where(a => a.Level == 1);
            var cityQuery = _areaRepository.GetAll().Where(a => a.Level == 2);
            var districtQuery = _areaRepository.GetAll().Where(a => a.Level == 3);

            var list = await query.PageBy(input)
                .Join(provinceQuery, address => address.ProvinceId, province => province.Id, (address, province) => new
                {
                    address.Id,
                    address.Street,
                    address.IsDefault,
                    Province = province.Name,
                    address.CityId,
                    address.DistrictId,
                    address.Receiver,
                    address.ContactPhoneNumber
                }).Join(cityQuery, address => address.CityId, city => city.Id, (address, city) => new
                {
                    address.Id,
                    address.Street,
                    address.IsDefault,
                    address.Province,
                    City = city.Name,
                    address.DistrictId,
                    address.Receiver,
                    address.ContactPhoneNumber
                }).Join(districtQuery, address => address.DistrictId, district => district.Id, (address, district) => new GetAddressesOutputDto
                {
                    Id = address.Id,
                    Street = address.Street,
                    IsDefault = address.IsDefault,
                    Province = address.Province,
                    City = address.City,
                    District = district.Name,
                    Receiver = address.Receiver,
                    ContactPhoneNumber = address.ContactPhoneNumber
                }).ToListAsync();

            return new PagedResultDto<GetAddressesOutputDto>(count, list);
        }
    }
}