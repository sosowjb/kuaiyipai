using System;
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
using Microsoft.EntityFrameworkCore;

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

        public OrderAppService(IRepository<OrderWaitingForPayment, Guid> paymentRepository, IRepository<OrderWaitingForSending, Guid> sendingRepository, IRepository<OrderWaitingForReceiving, Guid> receivingRepository, IRepository<OrderWaitingForEvaluating, Guid> evaluatingRepository, IRepository<OrderCompleted, Guid> completedRepository, IRepository<User, long> userRepository, IRepository<Entities.Address, Guid> addressRepository)
        {
            _paymentRepository = paymentRepository;
            _sendingRepository = sendingRepository;
            _receivingRepository = receivingRepository;
            _evaluatingRepository = evaluatingRepository;
            _completedRepository = completedRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public async Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsBuyer(GetWaitingForPaymentOrdersInputDto input)
        {
            var query = _paymentRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForPaymentOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForPaymentOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsBuyer(GetWaitingForSendingOrdersInputDto input)
        {
            var query = _sendingRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForSendingOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForSendingOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsBuyer(GetWaitingForReceivingOrdersInputDto input)
        {
            var query = _receivingRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForReceivingOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForReceivingOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsBuyer(GetWaitingForEvaluatingOrdersInputDto input)
        {
            var query = _evaluatingRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForEvaluatingOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsBuyer(GetCompletedOrdersInputDto input)
        {
            var query = _completedRepository.GetAll().Where(o => o.BuyerId == AbpSession.UserId.Value);
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

        public async Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsSeller(GetWaitingForPaymentOrdersInputDto input)
        {
            var query = _paymentRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForPaymentOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForPaymentOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsSeller(GetWaitingForSendingOrdersInputDto input)
        {
            var query = _sendingRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForSendingOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForSendingOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsSeller(GetWaitingForReceivingOrdersInputDto input)
        {
            var query = _receivingRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForReceivingOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForReceivingOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsSeller(GetWaitingForEvaluatingOrdersInputDto input)
        {
            var query = _evaluatingRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value);
            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(o => new GetWaitingForEvaluatingOrdersOutputDto
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

            return new PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>(count, list);
        }

        public async Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsSeller(GetCompletedOrdersInputDto input)
        {
            var query = _completedRepository.GetAll().Where(o => o.SellerId == AbpSession.UserId.Value);
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

        public async Task<GetEachTypeOrderCountOutputDto> GetEachTypeOrderCount()
        {
            try
            {
                if (!AbpSession.UserId.HasValue)
                {
                    throw new UserFriendlyException("用户未登录");
                }

                var output = new GetEachTypeOrderCountOutputDto();

                output.WaitPay = await _paymentRepository.CountAsync(t => t.SellerId == AbpSession.UserId.Value || t.BuyerId == AbpSession.UserId.Value);
                output.WaitReceive = await _receivingRepository.CountAsync(t => t.SellerId == AbpSession.UserId.Value || t.BuyerId == AbpSession.UserId.Value);
                output.WaitSend = await _sendingRepository.CountAsync(t => t.SellerId == AbpSession.UserId.Value || t.BuyerId == AbpSession.UserId.Value);

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