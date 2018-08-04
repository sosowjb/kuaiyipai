using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
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
        private readonly IRepository<ItemAuctioning, Guid> _itemAuctioningRepository;

        public BiddingAppService(IRepository<UserBiddingRecord, Guid> biddingRepository, IRepository<User, long> userRepository, IRepository<ItemAuctioning, Guid> itemAuctioningRepository)
        {
            _biddingRepository = biddingRepository;
            _userRepository = userRepository;
            _itemAuctioningRepository = itemAuctioningRepository;
        }

        public async Task<Guid> Bid(BidInputDto input)
        {
            var item = await _itemAuctioningRepository.GetAsync(input.ItemId);
            item.BiddingCount++;
            if (input.Price < item.HighestBiddingPrice + item.StepPrice)
            {
                throw new UserFriendlyException("出价必需大于或等于" + (item.HighestBiddingPrice + item.StepPrice));
            }

            if (input.Price % item.StepPrice > 0)
            {
                throw new UserFriendlyException("出价必需是加价幅度的整数倍");
            }
            item.HighestBiddingPrice = input.Price;
            await _itemAuctioningRepository.UpdateAsync(item);
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
                    Id = bidding.Id,
                    ItemId = bidding.ItemId,
                    BidTime = DateTime.Now,
                    Price = bidding.Price,
                    UserId = bidding.CreatorUserId.Value,
                    UserName = user.FullName,
                    NickName = user.NickName,
                    AvatarLink = user.AvatarLink
                }).ToListAsync();

            return new PagedResultDto<GetBiddingsOutputDto>(count, list);
        }
    }
}