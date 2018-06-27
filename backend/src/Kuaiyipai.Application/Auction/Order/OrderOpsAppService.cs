﻿using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Auction.Order.Dto;

namespace Kuaiyipai.Auction.Order
{
    public class OrderOpsAppService : ApplicationService, IOrderOpsAppService
    {
        private readonly IRepository<OrderWaitingForPayment, Guid> _paymentRepository;
        private readonly IRepository<OrderWaitingForSending, Guid> _sendingRepository;
        private readonly IRepository<OrderWaitingForReceiving, Guid> _receivingRepository;
        private readonly IRepository<OrderWaitingForEvaluating, Guid> _evaluatingRepository;
        private readonly IRepository<OrderCompleted, Guid> _completedRepository;
        private readonly IRepository<UserBalance, long> _balanceRepository;

        public OrderOpsAppService(IRepository<OrderWaitingForPayment, Guid> paymentRepository, IRepository<OrderWaitingForSending, Guid> sendingRepository, IRepository<OrderWaitingForReceiving, Guid> receivingRepository, IRepository<OrderWaitingForEvaluating, Guid> evaluatingRepository, IRepository<OrderCompleted, Guid> completedRepository, IRepository<UserBalance, long> balanceRepository)
        {
            _paymentRepository = paymentRepository;
            _sendingRepository = sendingRepository;
            _receivingRepository = receivingRepository;
            _evaluatingRepository = evaluatingRepository;
            _completedRepository = completedRepository;
            _balanceRepository = balanceRepository;
        }

        public async Task Pay(PayInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var order = await _paymentRepository.FirstOrDefaultAsync(input.OrderId);
            if (order == null)
            {
                throw new UserFriendlyException("找不到此订单");
            }

            if (order.BuyerId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("此订单不是你的订单");
            }

            var amountToPay = order.Amount;
            var balance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == AbpSession.UserId.Value);
            if (balance == null)
            {
                throw new UserFriendlyException("未查到余额或余额为空");
            }

            if (balance.TotalBalance - balance.FrozenBalance < amountToPay)
            {
                throw new UserFriendlyException("可用余额不足，请先充值");
            }

            // 把交易金额存入买家的冻结余额里
            balance.FrozenBalance += amountToPay;
            // 更新数据库
            await _balanceRepository.UpdateAsync(balance);

            // 更改订单状态
            order.PaidTime = DateTime.Now;
            await _paymentRepository.UpdateAsync(order);
            var sendingOrder = new OrderWaitingForSending
            {
                Id = order.Id,
                Code = order.Code,
                BuyerId = order.BuyerId,
                SellerId = order.SellerId,
                AddressId = order.AddressId,
                OrderTime = order.OrderTime,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount
            };
            await _sendingRepository.InsertAsync(sendingOrder);
        }

        public async Task Send(SendInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var order = await _sendingRepository.FirstOrDefaultAsync(input.OrderId);
            if (order == null)
            {
                throw new UserFriendlyException("找不到此订单");
            }

            if (order.SellerId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("此订单不是你的订单");
            }

            // 更改订单状态
            order.SentTime = DateTime.Now;
            order.DeliveryId = input.DeliveryId;
            await _sendingRepository.UpdateAsync(order);
            var receivingOrder = new OrderWaitingForReceiving
            {
                Id = order.Id,
                Code = order.Code,
                BuyerId = order.BuyerId,
                SellerId = order.SellerId,
                AddressId = order.AddressId,
                OrderTime = order.OrderTime,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount
            };
            await _receivingRepository.InsertAsync(receivingOrder);
        }

        public async Task Receive(ReceiveInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var order = await _receivingRepository.FirstOrDefaultAsync(input.OrderId);
            if (order == null)
            {
                throw new UserFriendlyException("找不到此订单");
            }

            if (order.BuyerId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("此订单不是你的订单");
            }

            // 更改订单状态
            order.ReceivedTime = DateTime.Now;
            await _receivingRepository.UpdateAsync(order);
            var evaluatingOrder = new OrderWaitingForEvaluating
            {
                Id = order.Id,
                Code = order.Code,
                BuyerId = order.BuyerId,
                SellerId = order.SellerId,
                AddressId = order.AddressId,
                OrderTime = order.OrderTime,
                Amount = order.Amount,
                DeliveryId = order.DeliveryId,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount
            };
            await _evaluatingRepository.InsertAsync(evaluatingOrder);
        }

        public async Task Evaluate(EvaluateInputDto input)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new UserFriendlyException("用户未登录");
            }

            var order = await _evaluatingRepository.FirstOrDefaultAsync(input.OrderId);
            if (order == null)
            {
                throw new UserFriendlyException("找不到此订单");
            }

            if (order.BuyerId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException("此订单不是你的订单");
            }

            // 更改订单状态
            order.EvaluatedTime = DateTime.Now;
            await _evaluatingRepository.UpdateAsync(order);
            var completedOrder = new OrderCompleted
            {
                Id = order.Id,
                Code = order.Code,
                BuyerId = order.BuyerId,
                SellerId = order.SellerId,
                AddressId = order.AddressId,
                OrderTime = order.OrderTime,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount,
                DeliveryId = order.DeliveryId,
                EvaluationLevel = input.EvaluationLevel,
                EvaluationContent = input.EvaluationContent
            };
            await _completedRepository.InsertAsync(completedOrder);
        }
    }
}