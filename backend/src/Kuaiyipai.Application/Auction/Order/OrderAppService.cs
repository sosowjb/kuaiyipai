using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Auction.Order.Dto;
using Kuaiyipai.Authorization.Users;
using Kuaiyipai.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kuaiyipai.Auction.Order
{
    public class OrderAppService : KuaiyipaiAppServiceBase, IOrderAppService
    {
        private readonly IRepository<OrderWaitingForPayment, Guid> _paymentRepository;
        private readonly IRepository<OrderWaitingForSending, Guid> _sendingRepository;
        private readonly IRepository<OrderWaitingForReceiving, Guid> _receivingRepository;
        private readonly IRepository<OrderWaitingForEvaluating, Guid> _evaluatingRepository;
        private readonly IRepository<OrderCompleted, Guid> _completedRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Entities.Address, Guid> _addressRepository;
        private readonly IRepository<ItemCompleted, Guid> _itemRepository;
        private readonly IRepository<ItemPic, Guid> _itemPicRepository;
        private readonly IConfigurationRoot _appConfiguration;

        public OrderAppService(IHostingEnvironment env, IRepository<OrderWaitingForPayment, Guid> paymentRepository, IRepository<OrderWaitingForSending, Guid> sendingRepository, IRepository<OrderWaitingForReceiving, Guid> receivingRepository, IRepository<OrderWaitingForEvaluating, Guid> evaluatingRepository, IRepository<OrderCompleted, Guid> completedRepository, IRepository<User, long> userRepository, IRepository<Entities.Address, Guid> addressRepository, IRepository<ItemCompleted, Guid> itemRepository, IRepository<ItemPic, Guid> itemPicRepository)
        {
            _paymentRepository = paymentRepository;
            _sendingRepository = sendingRepository;
            _receivingRepository = receivingRepository;
            _evaluatingRepository = evaluatingRepository;
            _completedRepository = completedRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _itemRepository = itemRepository;
            _itemPicRepository = itemPicRepository;
            _appConfiguration = env.GetAppConfiguration();
        }

        public async Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsBuyer(GetWaitingForPaymentOrdersInputDto input)
        {
            var query = _paymentRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value && o.PaidTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForPaymentOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForPaymentOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForPaymentOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsBuyer(GetWaitingForSendingOrdersInputDto input)
        {
            var query = _sendingRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value && o.SentTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForSendingOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForSendingOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForSendingOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsBuyer(GetWaitingForReceivingOrdersInputDto input)
        {
            var query = _receivingRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value && o.ReceivedTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForReceivingOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForReceivingOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForReceivingOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsBuyer(GetWaitingForEvaluatingOrdersInputDto input)
        {
            var query = _evaluatingRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value && o.EvaluatedTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForEvaluatingOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForEvaluatingOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsBuyer(GetCompletedOrdersInputDto input)
        {
            var query = _completedRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetCompletedOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetCompletedOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetCompletedOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsSeller(GetWaitingForPaymentOrdersInputDto input)
        {
            var query = _paymentRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value && o.PaidTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForPaymentOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForPaymentOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForPaymentOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsSeller(GetWaitingForSendingOrdersInputDto input)
        {
            var query = _sendingRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value && o.SentTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForSendingOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForSendingOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForSendingOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsSeller(GetWaitingForReceivingOrdersInputDto input)
        {
            var query = _receivingRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value && o.ReceivedTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForReceivingOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForReceivingOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForReceivingOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsSeller(GetWaitingForEvaluatingOrdersInputDto input)
        {
            var query = _evaluatingRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value && o.EvaluatedTime == null);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetWaitingForEvaluatingOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetWaitingForEvaluatingOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>(count, newList);
        }

        public async Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsSeller(GetCompletedOrdersInputDto input)
        {
            var query = _completedRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new
            {
                o.Id,
                o.Code,
                o.BuyerId,
                o.SellerId,
                o.AddressId,
                o.DeliveryId,
                o.OrderTime,
                o.Amount,
                o.ItemPriceAmount,
                o.ExpressCostAmount,
                o.ItemId
            }).ToListAsync();

            var itemIdList = list.Select(o => o.ItemId).ToList();

            var items = await _itemRepository.GetAll().Where(i => itemIdList.Contains(i.Id)).Select(i => new
            {
                ItemId = i.Id,
                ItemTitle = i.Title
            }).ToListAsync();
            var pics = await _itemPicRepository.GetAll().Where(i => itemIdList.Contains(i.ItemId) && i.IsCover).Select(i => new
            {
                i.ItemId,
                Url = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), i.Path), i.FileName + i.Extension).ToString()
            }).ToListAsync();

            var newList = new List<GetCompletedOrdersOutputDto>();
            foreach (var o in list)
            {
                newList.Add(new GetCompletedOrdersOutputDto
                {
                    Id = o.Id,
                    Code = o.Code,
                    BuyerId = o.BuyerId,
                    SellerId = o.SellerId,
                    AddressId = o.AddressId,
                    DeliveryId = o.DeliveryId,
                    OrderTime = o.OrderTime,
                    Amount = o.Amount,
                    ItemPriceAmount = o.ItemPriceAmount,
                    ExpressCostAmount = o.ExpressCostAmount,
                    ItemTitle = items.FirstOrDefault(i => i.ItemId == o.ItemId)?.ItemTitle,
                    ItemPicUrl = pics.FirstOrDefault(i => i.ItemId == o.ItemId)?.Url
                });
            }

            return new PagedResultDto<GetCompletedOrdersOutputDto>(count, newList);
        }

        public async Task<GetEachTypeOrderCountOutputDto> GetEachTypeOrderCountAsSeller()
        {
            try
            {
                if (!AbpSession.UserId.HasValue)
                {
                    throw new UserFriendlyException("用户未登录");
                }

                var output = new GetEachTypeOrderCountOutputDto();

                output.WaitPay = await _paymentRepository.CountAsync(t => t.SellerId == AbpSession.UserId.Value && t.PaidTime == null);
                output.WaitReceive = await _receivingRepository.CountAsync(t => t.SellerId == AbpSession.UserId.Value);
                output.WaitSend = await _sendingRepository.CountAsync(t => t.SellerId == AbpSession.UserId.Value);

                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<OrderDetailDto> GetOrder(Guid orderId)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            OrderDetailDto order = new OrderDetailDto();
            long sellerId, buyerId;
            Guid goodsId;

            var order5 = await _completedRepository.FirstOrDefaultAsync(orderId);
            if (order5 == null)
            {
                var order4 = await _evaluatingRepository.FirstOrDefaultAsync(orderId);
                if (order4 == null)
                {
                    var order3 = await _receivingRepository.FirstOrDefaultAsync(orderId);
                    if (order3 == null)
                    {
                        var order2 = await _sendingRepository.FirstOrDefaultAsync(orderId);
                        if (order2 == null)
                        {
                            var order1 = await _paymentRepository.FirstOrDefaultAsync(orderId);
                            if (order1 == null)
                            {
                                throw new UserFriendlyException("找不到订单");
                            }

                            order.Id = order1.Id;
                            order.OrderStatus = 1;
                            order.OrderTime = order1.OrderTime;
                            order.Price = order1.Amount;
                            order.PaidTime = order1.PaidTime;
                            order.AuctionNum = order1.Code;
                            order.DeliveryType = "快递";
                            order.DeliveryId = order1.DeliveryId;
                            sellerId = order1.SellerId;
                            buyerId = order1.BuyerId;
                            goodsId = order1.ItemId;
                        }
                        else
                        {
                            order.Id = order2.Id;
                            order.OrderStatus = 2;
                            order.OrderTime = order2.OrderTime;
                            order.Price = order2.Amount;
                            order.PaidTime = order2.PaidTime;
                            order.AuctionNum = order2.Code;
                            order.DeliveryType = "快递";
                            order.DeliveryId = order2.DeliveryId;
                            sellerId = order2.SellerId;
                            buyerId = order2.BuyerId;
                            goodsId = order2.ItemId;
                        }
                    }
                    else
                    {
                        order.Id = order3.Id;
                        order.OrderStatus = 3;
                        order.OrderTime = order3.OrderTime;
                        order.Price = order3.Amount;
                        order.PaidTime = order3.PaidTime;
                        order.AuctionNum = order3.Code;
                        order.DeliveryType = "快递";
                        order.DeliveryId = order3.DeliveryId;
                        sellerId = order3.SellerId;
                        buyerId = order3.BuyerId;
                        goodsId = order3.ItemId;
                    }
                }
                else
                {
                    order.Id = order4.Id;
                    order.OrderStatus = 4;
                    order.OrderTime = order4.OrderTime;
                    order.Price = order4.Amount;
                    order.PaidTime = order4.PaidTime;
                    order.AuctionNum = order4.Code;
                    order.DeliveryType = "快递";
                    order.DeliveryId = order4.DeliveryId;
                    sellerId = order4.SellerId;
                    buyerId = order4.BuyerId;
                    goodsId = order4.ItemId;
                }
            }
            else
            {
                order.Id = order5.Id;
                order.OrderStatus = 5;
                order.OrderTime = order5.OrderTime;
                order.Price = order5.Amount;
                order.PaidTime = order5.PaidTime;
                order.AuctionNum = order5.Code;
                order.DeliveryType = "快递";
                order.DeliveryId = order5.DeliveryId;
                sellerId = order5.SellerId;
                buyerId = order5.BuyerId;
                goodsId = order5.ItemId;
            }

            var seller = await _userRepository.FirstOrDefaultAsync(sellerId);
            order.SellerTel = seller.PhoneNumber;

            var address = await _addressRepository.FirstOrDefaultAsync(a => a.CreatorUserId == buyerId && a.IsDefault);
            order.BuyerName = address.Receiver;
            order.BuyerTel = address.ContactPhoneNumber;
            order.Address = address.Street;

            var item = await _itemRepository.FirstOrDefaultAsync(goodsId);
            if (item == null)
            {
                throw new UserFriendlyException("商品未找到");
            }

            order.GoodsName = item.Title;

            var itemPic = await _itemPicRepository.FirstOrDefaultAsync(i => i.ItemId == goodsId && i.IsCover);
            if (itemPic != null)
            {
                order.GoodsPicture = new Uri(new Uri(new Uri(_appConfiguration["App:ImageUrlPrefix"]), itemPic.Path), itemPic.FileName + itemPic.Extension).ToString();
            }
            else
            {
                order.GoodsPicture = "";
            }

            return order;
        }

        public async Task<GetEachTypeOrderCountOutputDto> GetEachTypeOrderCountAsBuyer()
        {
            try
            {
                if (!AbpSession.UserId.HasValue)
                {
                    throw new UserFriendlyException("用户未登录");
                }

                var output = new GetEachTypeOrderCountOutputDto();

                output.WaitPay = await _paymentRepository.CountAsync(t => t.BuyerId == AbpSession.UserId.Value && t.PaidTime == null);
                output.WaitReceive = await _receivingRepository.CountAsync(t => t.BuyerId == AbpSession.UserId.Value);
                output.WaitSend = await _sendingRepository.CountAsync(t => t.BuyerId == AbpSession.UserId.Value);

                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrders(GetCompletedOrdersInputDto input)
        {
            var query = _completedRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value || o.SellerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetCompletedOrdersOutputDto
            {
                Code = o.Code,
                BuyerId = o.BuyerId,
                SellerId = o.SellerId,
                AddressId = o.AddressId,
                DeliveryId = o.DeliveryId,
                OrderTime = o.OrderTime,
                Amount = o.Amount,
                ItemPriceAmount = o.ItemPriceAmount,
                ExpressCostAmount = o.ExpressCostAmount
            }).ToListAsync();

            return new PagedResultDto<GetCompletedOrdersOutputDto>(count, list);
        }
    }
}