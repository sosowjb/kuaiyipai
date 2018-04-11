using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Bidding.Dto;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Authorization.Users;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.Bidding
{
    public class BiddingAppService : KuaiyipaiAppServiceBase, IBiddingAppService
    {
        private readonly IRepository<UserBiddingRecord, Guid> _biddingRepository;
        private readonly IRepository<User, long> _userRepository;

        public BiddingAppService(IRepository<UserBiddingRecord, Guid> biddingRepository, IRepository<User, long> userRepository)
        {
            _biddingRepository = biddingRepository;
            _userRepository = userRepository;
        }

        public async Task<Guid> Bid(BidInputDto input)
        {
            return await _biddingRepository.InsertAndGetIdAsync(new UserBiddingRecord
            {
                ItemId = input.ItemId,
                Price = input.Price
            });
        }

        public async Task<PagedResultDto<GetBiddingsOutputDto>> GetBiddings(GetBiddingsInputDto input)
        {
            var query = _biddingRepository.GetAll().Where(b => b.ItemId == input.ItemId);
            var userQuery = _userRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            var count = await query.CountAsync();

            var list = await query.PageBy(input)
                .Join(userQuery, bidding => bidding.CreatorUserId, user => user.Id, (bidding, user) => new GetBiddingsOutputDto
                {
                    ItemId = bidding.ItemId,
                    BidTime = DateTime.Now,
                    Price = bidding.Price,
                    UserId = bidding.CreatorUserId.Value,
                    UserName = user.FullName
                }).ToListAsync();

            return new PagedResultDto<GetBiddingsOutputDto>(count, list);
        }
    }
}