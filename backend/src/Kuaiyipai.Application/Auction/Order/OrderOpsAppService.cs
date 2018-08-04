using System;
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
            order.AddressId = input.AddressId;
            await _paymentRepository.UpdateAsync(order);
            var sendingOrder = new OrderWaitingForSending
            {
                Id = order.Id,
                Code = order.Code,
                BuyerId = order.BuyerId,
                SellerId = order.SellerId,
                AddressId = order.AddressId,
                DeliveryId = order.DeliveryId,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount,
                ItemId = order.ItemId,
                EvaluationLevel = order.EvaluationLevel,
                EvaluationContent = order.EvaluationContent,
                OrderTime = order.OrderTime,
                PaidTime = DateTime.Now,
                SentTime = order.SentTime,
                ReceivedTime = order.ReceivedTime,
                EvaluatedTime = order.EvaluatedTime,
                CompletedTime = order.CompletedTime
            };
            await _sendingRepository.InsertAsync(sendingOrder);
            await _paymentRepository.DeleteAsync(input.OrderId);
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
                DeliveryId = input.DeliveryId,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount,
                ItemId = order.ItemId,
                EvaluationLevel = order.EvaluationLevel,
                EvaluationContent = order.EvaluationContent,
                OrderTime = order.OrderTime,
                PaidTime = order.PaidTime,
                SentTime = DateTime.Now,
                ReceivedTime = order.ReceivedTime,
                EvaluatedTime = order.EvaluatedTime,
                CompletedTime = order.CompletedTime
            };
            await _receivingRepository.InsertAsync(receivingOrder);
            await _sendingRepository.DeleteAsync(input.OrderId);
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
                DeliveryId = order.DeliveryId,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount,
                ItemId = order.ItemId,
                EvaluationLevel = order.EvaluationLevel,
                EvaluationContent = order.EvaluationContent,
                OrderTime = order.OrderTime,
                PaidTime = order.PaidTime,
                SentTime = order.SentTime,
                ReceivedTime = DateTime.Now,
                EvaluatedTime = order.EvaluatedTime,
                CompletedTime = order.CompletedTime
            };
            await _evaluatingRepository.InsertAsync(evaluatingOrder);
            await _receivingRepository.DeleteAsync(input.OrderId);

            // 余额变化
            var buyerBalance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == order.BuyerId);
            var sellerBalance = await _balanceRepository.FirstOrDefaultAsync(b => b.UserId == order.SellerId);

            buyerBalance.FrozenBalance -= order.Amount;
            await _balanceRepository.UpdateAsync(buyerBalance);
            if (sellerBalance == null)
            {
                sellerBalance = new UserBalance
                {
                    UserId = order.SellerId,
                    FrozenBalance = 0,
                    TotalBalance = order.Amount
                };
                await _balanceRepository.InsertAsync(sellerBalance);
            }
            else
            {
                sellerBalance.TotalBalance += order.Amount;
                await _balanceRepository.UpdateAsync(sellerBalance);
            }
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
                DeliveryId = order.DeliveryId,
                Amount = order.Amount,
                ItemPriceAmount = order.ItemPriceAmount,
                ExpressCostAmount = order.ExpressCostAmount,
                ItemId = order.ItemId,
                EvaluationLevel = input.EvaluationLevel,
                EvaluationContent = input.EvaluationContent,
                OrderTime = order.OrderTime,
                PaidTime = order.PaidTime,
                SentTime = order.SentTime,
                ReceivedTime = order.ReceivedTime,
                EvaluatedTime = DateTime.Now,
                CompletedTime = DateTime.Now
            };
            await _completedRepository.InsertAsync(completedOrder);
            await _evaluatingRepository.DeleteAsync(input.OrderId);
        }
    }
}