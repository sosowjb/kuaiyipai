using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Auction.Item.Dto;
using Kuaiyipai.Authorization.Users;
using Kuaiyipai.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kuaiyipai.Auction.Item
{
    public class ItemAppService : KuaiyipaiAppServiceBase, IItemAppService
    {
        private readonly IRepository<ItemDrafting, Guid> _itemDraftingRepository;
        private readonly IRepository<ItemAuctioning, Guid> _itemAuctioningRepository;
        private readonly IRepository<ItemCompleted, Guid> _itemCompletedRepository;
        private readonly IRepository<ItemTerminated, Guid> _itemTerminatedRepository;
        private readonly IRepository<ItemPic, Guid> _itemPicRepository;
        private readonly IRepository<Entities.Pillar> _pillarRepository;
        private readonly IRepository<Entities.Category> _categoryRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserBiddingRecord, Guid> _biddingRepository;
        private readonly IRepository<Entities.Address, Guid> _addressRepository;
        private readonly IRepository<OrderWaitingForPayment, Guid> _orderRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemAppService(IHostingEnvironment env, IRepository<ItemDrafting, Guid> itemDraftingRepository, IRepository<ItemAuctioning, Guid> itemAuctioningRepository, IRepository<ItemCompleted, Guid> itemCompletedRepository, IRepository<ItemTerminated, Guid> itemTerminatedRepository, IRepository<Entities.Pillar> pillarRepository, IRepository<Entities.Category> categoryRepository, IRepository<ItemPic, Guid> itemPicRepository, IHttpContextAccessor httpContextAccessor, IRepository<User, long> userRepository, IRepository<UserBiddingRecord, Guid> biddingRepository, IRepository<Entities.Address, Guid> addressRepository, IRepository<OrderWaitingForPayment, Guid> orderRepository)
        {
            _itemDraftingRepository = itemDraftingRepository;
            _itemAuctioningRepository = itemAuctioningRepository;
            _itemCompletedRepository = itemCompletedRepository;
            _itemTerminatedRepository = itemTerminatedRepository;
            _itemPicRepository = itemPicRepository;
            _httpContextAccessor = httpContextAccessor;
            _pillarRepository = pillarRepository;
            _categoryRepository = categoryRepository;
            _appConfiguration = env.GetAppConfiguration();
            _userRepository = userRepository;
            _biddingRepository = biddingRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Guid> CreateItem(CreateItemInputDto input)
        {
            var pillar = await _pillarRepository.GetAsync(input.PillarId);
            var category = await _categoryRepository.GetAsync(input.CategoryId);

            var item = new ItemDrafting
            {
                Code = pillar.Code + category.Code + DateTime.Now.Ticks,
                PillarId = input.PillarId,
                CategoryId = input.CategoryId,
                StartPrice = input.StartPrice,
                StepPrice = input.StepPrice,
                StartTime = input.StartTime,
                Deadline = input.Deadline,
                Title = input.Title,
                Description = input.Description,
                BiddingCount = 0,
                HighestBiddingPrice = 0
            };

            var itemId = await _itemDraftingRepository.InsertAndGetIdAsync(item);

            for (int i = 0; i < input.PictureList.Count; i++)
            {
                var p = await _itemPicRepository.GetAsync(input.PictureList[i].Id);
                p.IsCover = input.PictureList[i].IsCover;
                p.ItemId = itemId;
                p.Index = i + 1;
                await _itemPicRepository.UpdateAsync(p);
            }

            return itemId;
        }

        [UnitOfWork]
        public async Task StartAuction(EntityDto<Guid> input)
        {
            var item = await _itemDraftingRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("此ID的货品不存在");
            }

            var itemAuctioning = new ItemAuctioning
            {
                Id = item.Id,
                Code = item.Code,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StepPrice = item.StepPrice,
                StartTime = item.StartTime,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description,
                BiddingCount = item.BiddingCount,
                HighestBiddingPrice = item.HighestBiddingPrice
            };
            await _itemDraftingRepository.DeleteAsync(input.Id);
            await _itemAuctioningRepository.InsertAsync(itemAuctioning);
        }

        public async Task CompleteAuction(EntityDto<Guid> input)
        {
            // get bidding
            var itemAuctioning = await _itemAuctioningRepository.FirstOrDefaultAsync(input.Id);
            if (itemAuctioning == null)
            {
                throw new UserFriendlyException("此ID的货品不存在");
            }

            var bidding = await _biddingRepository.GetAll().Where(b => b.ItemId == itemAuctioning.Id)
                .OrderByDescending(b => b.CreationTime).FirstOrDefaultAsync();

            if (bidding == null)
            {
                throw new UserFriendlyException("没有人出价，请手动流拍");
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
            var random = new Random();
            if (buyer != null && seller != null)
            {
                var order = new OrderWaitingForPayment
                {
                    Code = DateTime.Now.Ticks + random.Next(0, 99999).ToString().PadLeft(5, '0'),
                    BuyerId = buyer.Id,
                    SellerId = seller.Id,
                    AddressId = address.Id,
                    Amount = bidding.Price,
                    ItemPriceAmount = bidding.Price,
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
            await _itemCompletedRepository.InsertAsync(itemComplete);
            await _itemAuctioningRepository.DeleteAsync(itemAuctioning.Id);
        }

        public async Task TerminateAuction(EntityDto<Guid> input)
        {
            var item = await _itemAuctioningRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("此ID的货品不存在");
            }

            var itemTerminated = new ItemTerminated
            {
                Id = item.Id,
                Code = item.Code,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StepPrice = item.StepPrice,
                StartTime = item.StartTime,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description,
                BiddingCount = item.BiddingCount,
                HighestBiddingPrice = item.HighestBiddingPrice
            };
            await _itemAuctioningRepository.DeleteAsync(input.Id);
            await _itemTerminatedRepository.InsertAsync(itemTerminated);
        }

        public async Task RecreateItem(EntityDto<Guid> input)
        {
            var item = await _itemTerminatedRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("此ID的货品不存在");
            }

            var pillar = await _pillarRepository.GetAsync(item.PillarId);
            var category = await _categoryRepository.GetAsync(item.CategoryId);

            var itemDrafting = new ItemDrafting
            {
                Code = pillar.Code + category.Code + DateTime.Now.Ticks,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StepPrice = item.StepPrice,
                StartTime = item.StartTime,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description,
                BiddingCount = 0,
                HighestBiddingPrice = 0
            };
            await _itemTerminatedRepository.DeleteAsync(input.Id);
            await _itemDraftingRepository.InsertAsync(itemDrafting);
        }

        public async Task DeleteItem(EntityDto<Guid> input)
        {
            var itemTerminated = await _itemTerminatedRepository.FirstOrDefaultAsync(input.Id);
            if (itemTerminated != null)
            {
                await _itemTerminatedRepository.DeleteAsync(input.Id);
            }

            var itemDrafting = await _itemDraftingRepository.FirstOrDefaultAsync(input.Id);
            if (itemDrafting != null)
            {
                await _itemDraftingRepository.DeleteAsync(input.Id);
            }
        }

        public async Task<PagedResultDto<GetMyDraftingItemsOutputDto>> GetMyDraftingItems(GetMyDraftingItemsInputDto input)
        {
            var query = _itemDraftingRepository.GetAll().Where(i => i.CreatorUserId == AbpSession.UserId.Value);
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new
                {
                    item.Id,
                    Pillar = pillar.Name,
                    item.CategoryId,
                    item.Title,
                    item.Description,
                    item.StartPrice,
                    item.StepPrice,
                    item.StartTime,
                    item.Deadline
                }).Join(categoryQuery, item => item.CategoryId, category => category.Id, (item, category) => new GetMyDraftingItemsOutputDto
                {
                    Id = item.Id,
                    Pillar = item.Pillar,
                    Category = category.Name,
                    Title = item.Title,
                    Description = item.Description,
                    StartPrice = item.StartPrice,
                    StepPrice = item.StepPrice,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline
                }).ToListAsync();

            return new PagedResultDto<GetMyDraftingItemsOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetMyAuctionItemsOutputDto>> GetMyAuctionItems(GetMyAuctionItemsInputDto input)
        {
            var query = _itemAuctioningRepository.GetAll().Where(i => i.CreatorUserId == AbpSession.UserId.Value);
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new
                {
                    item.Id,
                    Pillar = pillar.Name,
                    item.CategoryId,
                    item.Title,
                    item.Description,
                    item.StartPrice,
                    item.StepPrice,
                    item.StartTime,
                    item.Deadline,
                    item.BiddingCount,
                    item.HighestBiddingPrice
                }).Join(categoryQuery, item => item.CategoryId, category => category.Id, (item, category) => new GetMyAuctionItemsOutputDto
                {
                    Id = item.Id,
                    Pillar = item.Pillar,
                    Category = category.Name,
                    Title = item.Title,
                    Description = item.Description,
                    StartPrice = item.StartPrice,
                    StepPrice = item.StepPrice,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    BiddingCount = item.BiddingCount,
                    HighestBiddingPrice = item.HighestBiddingPrice
                }).ToListAsync();

            return new PagedResultDto<GetMyAuctionItemsOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetMyCompletedItemsOutputDto>> GetMyCompletedItems(GetMyCompletedItemsInputDto input)
        {
            var query = _itemCompletedRepository.GetAll().Where(i => i.CreatorUserId == AbpSession.UserId.Value);
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new
                {
                    item.Id,
                    Pillar = pillar.Name,
                    item.CategoryId,
                    item.Title,
                    item.Description,
                    item.StartPrice,
                    item.StepPrice,
                    item.StartTime,
                    item.Deadline,
                    item.BiddingCount,
                    item.HighestBiddingPrice
                }).Join(categoryQuery, item => item.CategoryId, category => category.Id, (item, category) => new GetMyCompletedItemsOutputDto
                {
                    Id = item.Id,
                    Pillar = item.Pillar,
                    Category = category.Name,
                    Title = item.Title,
                    Description = item.Description,
                    StartPrice = item.StartPrice,
                    StepPrice = item.StepPrice,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    BiddingCount = item.BiddingCount,
                    HighestBiddingPrice = item.HighestBiddingPrice
                }).ToListAsync();

            return new PagedResultDto<GetMyCompletedItemsOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetMyTerminatedItemsOutputDto>> GetMyTerminatedItems(GetMyTerminatedItemsInputDto input)
        {
            var query = _itemTerminatedRepository.GetAll().Where(i => i.CreatorUserId == AbpSession.UserId.Value);
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new
                {
                    item.Id,
                    Pillar = pillar.Name,
                    item.CategoryId,
                    item.Title,
                    item.Description,
                    item.StartPrice,
                    item.StepPrice,
                    item.StartTime,
                    item.Deadline,
                    item.BiddingCount,
                    item.HighestBiddingPrice
                })
                .Join(categoryQuery, item => item.CategoryId, category => category.Id, (item, category) => new GetMyTerminatedItemsOutputDto
                {
                    Id = item.Id,
                    Pillar = item.Pillar,
                    Category = category.Name,
                    Title = item.Title,
                    Description = item.Description,
                    StartPrice = item.StartPrice,
                    StepPrice = item.StepPrice,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    BiddingCount = item.BiddingCount,
                    HighestBiddingPrice = item.HighestBiddingPrice
                }).ToListAsync();

            return new PagedResultDto<GetMyTerminatedItemsOutputDto>(count, list);
        }
        public async Task<GetDraftingItemOutputDto> GetDraftingItem(EntityDto<Guid> input)
        {
            var item = await _itemDraftingRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("商品不存在");
            }
            return new GetDraftingItemOutputDto
            {
                Id = item.Id,
                Code = item.Code,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StartTime = item.StartTime,
                StepPrice = item.StepPrice,
                PriceLimit = item.PriceLimit,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description
            };
        }

        public async Task<GetAuctionItemOutputDto> GetAuctionItem(EntityDto<Guid> input)
        {
            var item = await _itemAuctioningRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("商品不存在");
            }
            return new GetAuctionItemOutputDto
            {
                Id = item.Id,
                Code = item.Code,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StartTime = item.StartTime,
                StepPrice = item.StepPrice,
                PriceLimit = item.PriceLimit,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description,
                BiddingCount = item.BiddingCount,
                HighestBiddingPrice = item.HighestBiddingPrice
            };
        }

        public async Task<GetCompletedItemOutputDto> GetCompletedItem(EntityDto<Guid> input)
        {
            var item = await _itemCompletedRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("商品不存在");
            }
            return new GetCompletedItemOutputDto
            {
                Id = item.Id,
                Code = item.Code,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StartTime = item.StartTime,
                StepPrice = item.StepPrice,
                PriceLimit = item.PriceLimit,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description,
                BiddingCount = item.BiddingCount,
                HighestBiddingPrice = item.HighestBiddingPrice
            };
        }

        public async Task<GetTerminatedItemOutputDto> GetTerminatedItem(EntityDto<Guid> input)
        {
            var item = await _itemTerminatedRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("商品不存在");
            }
            return new GetTerminatedItemOutputDto
            {
                Id = item.Id,
                Code = item.Code,
                PillarId = item.PillarId,
                CategoryId = item.CategoryId,
                StartPrice = item.StartPrice,
                StartTime = item.StartTime,
                StepPrice = item.StepPrice,
                PriceLimit = item.PriceLimit,
                Deadline = item.Deadline,
                Title = item.Title,
                Description = item.Description,
                BiddingCount = item.BiddingCount,
                HighestBiddingPrice = item.HighestBiddingPrice
            };
        }

        public async Task<GetItemOutputDto> GetItem(EntityDto<Guid> input)
        {
            var item1 = await _itemDraftingRepository.GetAll().Join(_userRepository.GetAll(), item => item.CreatorUserId, user => user.Id, (item, user) => new { item, user }).FirstOrDefaultAsync(o => o.item.Id == input.Id);
            if (item1 == null)
            {
                var item2 = await _itemAuctioningRepository.GetAll().Join(_userRepository.GetAll(), item => item.CreatorUserId, user => user.Id, (item, user) => new { item, user }).FirstOrDefaultAsync(o => o.item.Id == input.Id);
                if (item2 == null)
                {
                    var item3 = await _itemCompletedRepository.GetAll().Join(_userRepository.GetAll(), item => item.CreatorUserId, user => user.Id, (item, user) => new { item, user }).FirstOrDefaultAsync(o => o.item.Id == input.Id);
                    if (item3 == null)
                    {
                        var item4 = await _itemTerminatedRepository.GetAll().Join(_userRepository.GetAll(), item => item.CreatorUserId, user => user.Id, (item, user) => new { item, user }).FirstOrDefaultAsync(o => o.item.Id == input.Id);
                        if (item4 == null)
                        {
                            throw new UserFriendlyException("商品不存在");
                        }

                        return new GetItemOutputDto
                        {
                            Id = item4.item.Id,
                            Code = item4.item.Code,
                            PillarId = item4.item.PillarId,
                            CategoryId = item4.item.CategoryId,
                            StartPrice = item4.item.StartPrice,
                            StartTime = item4.item.StartTime,
                            StepPrice = item4.item.StepPrice,
                            PriceLimit = item4.item.PriceLimit,
                            Deadline = item4.item.Deadline,
                            Title = item4.item.Title,
                            Description = item4.item.Description,
                            Status = "Terminated",
                            BiddingCount = item4.item.BiddingCount,
                            HighestBiddingPrice = item4.item.HighestBiddingPrice,
                            CreatorUserId = item4.item.CreatorUserId,
                            Avator = item4.user.AvatarLink,
                            NikeName = item4.user.NickName
                        };
                    }

                    return new GetItemOutputDto
                    {
                        Id = item3.item.Id,
                        Code = item3.item.Code,
                        PillarId = item3.item.PillarId,
                        CategoryId = item3.item.CategoryId,
                        StartPrice = item3.item.StartPrice,
                        StartTime = item3.item.StartTime,
                        StepPrice = item3.item.StepPrice,
                        PriceLimit = item3.item.PriceLimit,
                        Deadline = item3.item.Deadline,
                        Title = item3.item.Title,
                        Description = item3.item.Description,
                        Status = "Completed",
                        BiddingCount = item3.item.BiddingCount,
                        HighestBiddingPrice = item3.item.HighestBiddingPrice,
                        CreatorUserId = item3.item.CreatorUserId,
                        Avator = item3.user.AvatarLink,
                        NikeName = item3.user.NickName

                    };
                }

                return new GetItemOutputDto
                {
                    Id = item2.item.Id,
                    Code = item2.item.Code,
                    PillarId = item2.item.PillarId,
                    CategoryId = item2.item.CategoryId,
                    StartPrice = item2.item.StartPrice,
                    StartTime = item2.item.StartTime,
                    StepPrice = item2.item.StepPrice,
                    PriceLimit = item2.item.PriceLimit,
                    Deadline = item2.item.Deadline,
                    Title = item2.item.Title,
                    Description = item2.item.Description,
                    Status = "Auctioning",
                    BiddingCount = item2.item.BiddingCount,
                    HighestBiddingPrice = item2.item.HighestBiddingPrice,
                    CreatorUserId = item2.item.CreatorUserId,
                    Avator = item2.user.AvatarLink,
                    NikeName = item2.user.NickName
                };
            }
            try
            {
                return new GetItemOutputDto
                {
                    Id = item1.item.Id,
                    Code = item1.item.Code,
                    PillarId = item1.item.PillarId,
                    CategoryId = item1.item.CategoryId,
                    StartPrice = item1.item.StartPrice,
                    StartTime = item1.item.StartTime,
                    StepPrice = item1.item.StepPrice,
                    PriceLimit = item1.item.PriceLimit,
                    Deadline = item1.item.Deadline,
                    Title = item1.item.Title,
                    Description = item1.item.Description,
                    Status = "Drafting",
                    BiddingCount = item1.item.BiddingCount,
                    HighestBiddingPrice = item1.item.HighestBiddingPrice,
                    CreatorUserId = item1.item.CreatorUserId,
                    Avator = item1.user.AvatarLink,
                    NikeName = item1.user.NickName
                };
            }
            catch (Exception wx)
            {
                var message = wx;
                throw;
            }

        }

        public async Task<PagedResultDto<GetAuctionItemsOutputDto>> GetAuctionItemsEx(GetAuctionItemsExInputDto input)
        {
            var query = _itemAuctioningRepository.GetAll()
                .Where(i =>
                    (!input.PillarId.HasValue || i.PillarId == input.PillarId) &&
                    (!input.CategoryId.HasValue || i.CategoryId == input.CategoryId) &&
                    (!input.PublishTimeStart.HasValue || i.CreationTime >= input.PublishTimeStart) &&
                    (!input.PublishTimeEnd.HasValue || i.CreationTime <= input.PublishTimeEnd) &&
                    (!input.DeadlineStart.HasValue || i.Deadline >= input.DeadlineStart) &&
                    (!input.DeadlineEnd.HasValue || i.Deadline <= input.DeadlineEnd) &&
                    (!input.PriceStart.HasValue || i.StartPrice >= input.PriceStart) &&
                    (!input.PriceEnd.HasValue || i.StartPrice <= input.PriceEnd) &&
                    (string.IsNullOrEmpty(input.Title) || i.Title.Contains(input.Title)));
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();
            var itempicQuery = _itemPicRepository.GetAll().Where(o => o.IsCover);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            var count = await query.CountAsync();

            var list = await query.PageBy(input)
                         .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new { item, pillar })
                         .Join(categoryQuery, items => items.item.CategoryId, category => category.Id, (items, category) => new { items, category })
                         .Join(itempicQuery, itemss => itemss.items.item.Id, itempic => itempic.ItemId, (itemss, itempic) => new GetAuctionItemsOutputDto
                         {

                             Id = itemss.items.item.Id,
                             Pillar = itemss.items.pillar.Name,
                             Category = itemss.category.Name,
                             Title = itemss.items.item.Title,
                             StartPrice = itemss.items.item.StartPrice,
                             StepPrice = itemss.items.item.StepPrice,
                             StartTime = itemss.items.item.StartTime,
                             Deadline = itemss.items.item.Deadline,
                             CoverPic = new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), Path.Combine(itempic.Path, itempic.FileName + itempic.Extension)).ToString(),
                             CoverPicHeight = itempic.Height,
                             CoverPicWidth = itempic.Width,
                             CurrentPrice = itemss.items.item.HighestBiddingPrice,
                             biddingCount = itemss.items.item.BiddingCount
                         }).ToListAsync();
            return new PagedResultDto<GetAuctionItemsOutputDto>(count, list);
        }

        public async Task<UploadPictureOutputDto> UploadPicture()
        {
            var files = _httpContextAccessor.HttpContext.Request.Form.Files;
            if (files.Count != 1)
            {
                throw new UserFriendlyException("未上传图片");
            }

            try
            {
                var now = DateTime.Now;
                var relativePath = Path.Combine(now.Year.ToString(), now.Month.ToString(), now.Day.ToString(),
                    now.Hour.ToString());
                var filePath = Path.Combine(_appConfiguration["App:ImagePhysicalPath"], relativePath);
                var fileName = now.ToString("yyyyMMddhhmmssffff") + "_" + AbpSession.UserId;
                var ext = ".jpg";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var file = files[0];
                using (var stream = new FileStream(Path.Combine(filePath, fileName + ext), FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }

                var pic = new ItemPic
                {
                    Path = relativePath,
                    FileName = fileName,
                    Size = file.Length,
                    Extension = ext
                };
                var id = await _itemPicRepository.InsertAndGetIdAsync(pic);
                return new UploadPictureOutputDto { Id = id };
            }
            catch
            {
                throw new UserFriendlyException("图片上传失败");
            }
        }

        public async Task<ListResultDto<GetItemPicturesOutputDto>> GetItemPictures(GetItemPicturesInputDto input)
        {
            var list = await _itemPicRepository.GetAll().Where(p => p.ItemId == input.Id).OrderBy(p => p.Index).Select(p => new GetItemPicturesOutputDto
            {
                Id = p.Id,
                Height = p.Height,
                Width = p.Width,
                IsCover = p.IsCover,
                Url = new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), Path.Combine(p.Path, p.FileName + p.Extension)).ToString()
            }).ToListAsync();
            return new ListResultDto<GetItemPicturesOutputDto>(list);
        }

        public async Task<GetItemPicturesOutputDto> GetCoverPicture(GetItemPicturesInputDto input)
        {
            var pic = await _itemPicRepository.FirstOrDefaultAsync(p => p.ItemId == input.Id && p.IsCover);
            return new GetItemPicturesOutputDto
            {
                Id = pic.Id,
                Height = pic.Height,
                Width = pic.Width,
                IsCover = pic.IsCover,
                Url = new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), Path.Combine(pic.Path, pic.FileName + pic.Extension)).ToString()
            };
        }

        public async Task DeleteItemPicture(EntityDto<Guid> input)
        {
            var pic = await _itemPicRepository.GetAsync(input.Id);
            if (pic == null)
            {
                throw new UserFriendlyException("图片不存在");
            }

            var path = Path.Combine(_appConfiguration["App:ImagePhysicalPath"], pic.Path, pic.FileName + pic.Extension);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            await _itemPicRepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetAuctionItemsOutputDto>> GetAuctionItems(GetAuctionItemsInputDto input)
        {
            var query = _itemAuctioningRepository.GetAll();
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();
            var itempicQuery = _itemPicRepository.GetAll().Where(o => o.IsCover);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            var count = await query.CountAsync();

            var list = await query.PageBy(input)
                         .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new { item, pillar })
                         .Join(categoryQuery, items => items.item.CategoryId, category => category.Id, (items, category) => new { items, category })
                         .Join(itempicQuery, itemss => itemss.items.item.Id, itempic => itempic.ItemId, (itemss, itempic) => new GetAuctionItemsOutputDto
                         {

                             Id = itemss.items.item.Id,
                             Pillar = itemss.items.pillar.Name,
                             Category = itemss.category.Name,
                             Title = itemss.items.item.Title,
                             StartPrice = itemss.items.item.StartPrice,
                             StepPrice = itemss.items.item.StepPrice,
                             StartTime = itemss.items.item.StartTime,
                             Deadline = itemss.items.item.Deadline,
                             CoverPic = new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), Path.Combine(itempic.Path, itempic.FileName + itempic.Extension)).ToString(),
                             CoverPicHeight = itempic.Height,
                             CoverPicWidth = itempic.Width,
                             CurrentPrice = itemss.items.item.HighestBiddingPrice,
                             biddingCount = itemss.items.item.BiddingCount
                         }).ToListAsync();
            return new PagedResultDto<GetAuctionItemsOutputDto>(count, list);

        }
    }
}