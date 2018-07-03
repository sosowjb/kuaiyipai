using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Authorization.Users;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction
{
    public class AuctionManager : KuaiyipaiDomainServiceBase, IAuctionManager
    {
        private readonly IRepository<ItemAuctioning, Guid> _itemAuctioningRepository;
        private readonly IRepository<ItemCompleted, Guid> _itemCompleteRepository;
        private readonly IRepository<OrderWaitingForPayment, Guid> _orderRepository;
        private readonly IRepository<UserBiddingRecord, Guid> _biddingRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Address, Guid> _addressRepository;

        public AuctionManager(IRepository<ItemAuctioning, Guid> itemAuctioningRepository, IRepository<ItemCompleted, Guid> itemCompleteRepository, IRepository<OrderWaitingForPayment, Guid> orderRepository, IRepository<UserBiddingRecord, Guid> biddingRepository, IRepository<User, long> userRepository, IRepository<Address, Guid> addressRepository)
        {
            _itemAuctioningRepository = itemAuctioningRepository;
            _itemCompleteRepository = itemCompleteRepository;
            _orderRepository = orderRepository;
            _biddingRepository = biddingRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        [UnitOfWork]
        public async Task CompleteAuctionAndGenerateOrder()
        {
            var itemsToBeCompletedQuery = _itemAuctioningRepository.GetAll().Where(i => i.Deadline <= DateTime.Now);
            var random = new Random();
            foreach (var itemAuctioning in itemsToBeCompletedQuery)
            {
                // get bidding
                var bidding = await _biddingRepository.GetAll().Where(b => b.ItemId == itemAuctioning.Id)
                    .OrderByDescending(b => b.CreationTime).FirstOrDefaultAsync();

                if (bidding == null)
                {
                    continue;
                }

                // get buyer and seller
                User buyer = null, seller = null;
                if (bidding.CreatorUserId != null)
                    buyer = await _userRepository.GetAsync(bidding.CreatorUserId.Value);
                if (itemAuctioning.CreatorUserId != null)
                    seller = await _userRepository.GetAsync(itemAuctioning.CreatorUserId.Value);

                // get address
                var address = await _addressRepository.GetAll().Where(a => a.CreatorUserId.Value == buyer.Id && a.IsDefault).FirstOrDefaultAsync();

                // create order
                if (buyer != null && seller != null)
                {
                    var order = new OrderWaitingForPayment
                    {
                        Code = DateTime.Now.Ticks + random.Next(0, 99999).ToString().PadLeft(5, '0'),
                        AddressId = address.Id,
                        Amount = bidding.Price,
                        ItemPriceAmount = bidding.Price,
                        BuyerId = buyer.Id,
                        SellerId = seller.Id,
                        OrderTime = DateTime.Now,
                        ItemId = itemAuctioning.Id
                    };
                    await _orderRepository.InsertAsync(order);
                }

                // change item status
                var itemComplete = new ItemCompleted
                {
                    Id = itemAuctioning.Id,
                    Code = itemAuctioning.Code,
                    PillarId = itemAuctioning.PillarId,
                    CategoryId = itemAuctioning.CategoryId,
                    StartPrice = itemAuctioning.StartPrice,
                    StepPrice = itemAuctioning.StepPrice,
                    PriceLimit = itemAuctioning.PriceLimit,
                    StartTime = itemAuctioning.StartTime,
                    Title = itemAuctioning.Title,
                    Description = itemAuctioning.Description,
                    BiddingCount = itemAuctioning.BiddingCount,
                    HighestBiddingPrice = itemAuctioning.HighestBiddingPrice
                };
                await _itemCompleteRepository.InsertAsync(itemComplete);
                await _itemAuctioningRepository.DeleteAsync(itemAuctioning.Id);
            }
        }
    }
}