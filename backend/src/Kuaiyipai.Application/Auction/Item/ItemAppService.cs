using System;
using System.Drawing;
using System.Drawing.Imaging;
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
using Kuaiyipai.Configuration;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IConfigurationRoot _appConfiguration;

        public ItemAppService(IHostingEnvironment env, IRepository<ItemDrafting, Guid> itemDraftingRepository, IRepository<ItemAuctioning, Guid> itemAuctioningRepository, IRepository<ItemCompleted, Guid> itemCompletedRepository, IRepository<ItemTerminated, Guid> itemTerminatedRepository, IRepository<Entities.Pillar> pillarRepository, IRepository<Entities.Category> categoryRepository, IRepository<ItemPic, Guid> itemPicRepository)
        {
            _itemDraftingRepository = itemDraftingRepository;
            _itemAuctioningRepository = itemAuctioningRepository;
            _itemCompletedRepository = itemCompletedRepository;
            _itemTerminatedRepository = itemTerminatedRepository;
            _itemPicRepository = itemPicRepository;
            _pillarRepository = pillarRepository;
            _categoryRepository = categoryRepository;
            _appConfiguration = env.GetAppConfiguration();
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
                Description = item.Description
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
                Description = item.Description
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
                Description = item.Description
            };
        }

        public async Task<UploadPictureOutputDto> UploadPicture(UploadPictureInputDto input)
        {
            try
            {
                var now = DateTime.Now;
                var relativePath = Path.Combine(now.Year.ToString(), now.Month.ToString(), now.Day.ToString(), now.Hour.ToString());
                var filePath = Path.Combine(_appConfiguration["App:ImagePhysicalPath"], relativePath);
                var fileName = now.ToString("yyyyMMddhhmmssffff") + "_" + AbpSession.UserId;
                var ext = ".jpg";

                var base64 = (input.Base64.IndexOf(',') == -1 ? input.Base64 : input.Base64.Substring(input.Base64.IndexOf(',') + 1)).Trim('\0');
                byte[] arr = Convert.FromBase64String(base64);
                long length = arr.Length;
                int height;
                int width;
                using (MemoryStream ms = new MemoryStream(arr))
                {
                    Bitmap bmp = new Bitmap(ms);
                    height = bmp.Height;
                    width = bmp.Width;
                    Bitmap bmp2 = new Bitmap(bmp, bmp.Width, bmp.Height);
                    Graphics draw = Graphics.FromImage(bmp2);
                    draw.DrawImage(bmp, 0, 0);
                    draw.Dispose();
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    bmp2.Save(Path.Combine(filePath, fileName + ext), ImageFormat.Jpeg);
                    ms.Close();
                }

                var pic = new ItemPic
                {
                    Path = relativePath,
                    FileName = fileName,
                    Size = length,
                    Extension = ext,
                    Height = height,
                    Width = width
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
            var itempicQuery = _itemPicRepository.GetAll();
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }
            var count1 = await categoryQuery.CountAsync();
            var count2 = await pillarQuery.CountAsync();
            var count = await query.CountAsync();
            var list = await query.PageBy(input).Join(pillarQuery, item => item.PillarId, pillar => pillar.Id, (item, pillar) => new
            {
                item.Id,
                Pillar = pillar.Name,
                fengmian = itempicQuery.Where(i => i.ItemId == item.Id).First(),
                item.CategoryId,
                item.Title,
                item.StartPrice,
                item.StepPrice,
                item.StartTime,
                item.Deadline
            })
                .Join(categoryQuery, item => item.CategoryId, category => category.Id, (item, category) => new GetAuctionItemsOutputDto
                {
                    Id = item.Id,
                    Pillar = item.Pillar,
                    Category = category.Name,
                    Title = item.Title,
                    StartPrice = item.StartPrice,
                    StepPrice = item.StepPrice,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    CoverPic = item.fengmian == null ? "" : item.fengmian.Path,
                    CoverPicWidth = item.fengmian == null ? 0 : item.fengmian.Width,
                    CoverPicHeight = item.fengmian == null ? 0 : item.fengmian.Height
                }).ToListAsync();

            return new PagedResultDto<GetAuctionItemsOutputDto>(count, list);
        }
    }
}