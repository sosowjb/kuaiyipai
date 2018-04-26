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
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Auction.Item.Dto;
using Microsoft.EntityFrameworkCore;

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

        public ItemAppService(IRepository<ItemDrafting, Guid> itemDraftingRepository, IRepository<ItemAuctioning, Guid> itemAuctioningRepository, IRepository<ItemCompleted, Guid> itemCompletedRepository, IRepository<ItemTerminated, Guid> itemTerminatedRepository, IRepository<ItemPic, Guid> itemPicRepository, IRepository<Entities.Pillar> pillarRepository, IRepository<Entities.Category> categoryRepository)
        {
            _itemDraftingRepository = itemDraftingRepository;
            _itemAuctioningRepository = itemAuctioningRepository;
            _itemCompletedRepository = itemCompletedRepository;
            _itemTerminatedRepository = itemTerminatedRepository;
            _itemPicRepository = itemPicRepository;
            _pillarRepository = pillarRepository;
            _categoryRepository = categoryRepository;
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
                Description = input.Description
            };

            return await _itemDraftingRepository.InsertAndGetIdAsync(item);
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
                Description = item.Description
            };
            await _itemDraftingRepository.DeleteAsync(input.Id);
            await _itemAuctioningRepository.InsertAsync(itemAuctioning);
        }

        public async Task CompleteAuction(EntityDto<Guid> input)
        {
            var item = await _itemAuctioningRepository.FirstOrDefaultAsync(input.Id);
            if (item == null)
            {
                throw new UserFriendlyException("此ID的货品不存在");
            }

            var itemCompleted = new ItemCompleted
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
                Description = item.Description
            };
            await _itemAuctioningRepository.DeleteAsync(input.Id);
            await _itemCompletedRepository.InsertAsync(itemCompleted);
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
                Description = item.Description
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
                Description = item.Description
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
                    item.Deadline
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
                    Deadline = item.Deadline
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
                    item.Deadline
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
                    Deadline = item.Deadline
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
                    item.Deadline
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
                    Deadline = item.Deadline
                }).ToListAsync();

            return new PagedResultDto<GetMyTerminatedItemsOutputDto>(count, list);
        }



        public async Task<PagedResultDto<GetAuctionItemsOutputDto>> GetAuctionItems(GetAuctionItemsInputDto input)
        {
            var query = _itemAuctioningRepository.GetAll();
            var pillarQuery = _pillarRepository.GetAll();
            var categoryQuery = _categoryRepository.GetAll();
            var itempicQuery = _itemPicRepository.GetAll().Where(o=>o.IsCover==true);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            var count1 = await categoryQuery.CountAsync();
            var count2 = await pillarQuery.CountAsync();
            var count = await query.CountAsync();
            var list = await query.PageBy(input)
                         .Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new { item, pillar })
                         .Join(categoryQuery, items => items.item.CategoryId, category => category.Id, (items, category) => new { items, category })
                         .Join(itempicQuery, items => items.items.item.Id, itempic => itempic.ItemId,(items, itempic) => new GetAuctionItemsOutputDto
                         {

                             Id = items.items.item.Id,
                             Pillar = items.items.pillar.Name,
                             Category = items.category.Name,
                             Title = items.items.item.Title,
                             StartPrice = items.items.item.StartPrice,
                             StepPrice = items.items.item.StepPrice,
                             StartTime = items.items.item.StartTime,
                             Deadline = items.items.item.Deadline,
                             CoverPic=itempic.Path,
                             CoverPicHeight=itempic.Height,
                             CoverPicWidth=itempic.Width
                         }).ToListAsync();

            return new PagedResultDto<GetAuctionItemsOutputDto>(count, list);
        }
    }
}