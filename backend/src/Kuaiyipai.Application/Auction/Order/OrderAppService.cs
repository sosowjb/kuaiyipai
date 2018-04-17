using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
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

        public Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsBuyer(GetWaitingForPaymentOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsBuyer(GetWaitingForSendingOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsBuyer(GetWaitingForReceivingOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsBuyer(GetWaitingForEvaluatingOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsBuyer(GetCompletedOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsSeller(GetWaitingForPaymentOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsSeller(GetWaitingForSendingOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsSeller(GetWaitingForReceivingOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsSeller(GetWaitingForEvaluatingOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsSeller(GetCompletedOrdersInputDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<GetEachTypeOrderCountOutputDto> GetEachTypeOrderCount(GetEachTypeOrderCountInputDto inputDto)
        {
            try
            {
                var output = new GetEachTypeOrderCountOutputDto();

                output.WaitPay = await _paymentRepository.CountAsync(t => inputDto.UserType == 0 ? t.SellerId == inputDto.UserId : t.BuyerId == inputDto.UserId);
                output.WaitReceive = await _receivingRepository.CountAsync(t => inputDto.UserType == 0 ? t.SellerId == inputDto.UserId : t.BuyerId == inputDto.UserId);
                output.WaitSend = await _sendingRepository.CountAsync(t => inputDto.UserType == 0 ? t.SellerId == inputDto.UserId : t.BuyerId == inputDto.UserId);

                return output;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}