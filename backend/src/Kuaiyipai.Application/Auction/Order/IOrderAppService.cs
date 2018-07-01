using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Order.Dto;

namespace Kuaiyipai.Auction.Order
{
    public interface IOrderAppService
    {
        Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsBuyer(GetWaitingForPaymentOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsBuyer(GetWaitingForSendingOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsBuyer(GetWaitingForReceivingOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsBuyer(GetWaitingForEvaluatingOrdersInputDto input);

        Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsBuyer(GetCompletedOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForPaymentOrdersOutputDto>> GetWaitingForPaymentOrdersAsSeller(GetWaitingForPaymentOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForSendingOrdersOutputDto>> GetWaitingForSendingOrdersAsSeller(GetWaitingForSendingOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForReceivingOrdersOutputDto>> GetWaitingForReceivingOrdersAsSeller(GetWaitingForReceivingOrdersInputDto input);

        Task<PagedResultDto<GetWaitingForEvaluatingOrdersOutputDto>> GetWaitingForEvaluatingOrdersAsSeller(GetWaitingForEvaluatingOrdersInputDto input);

        Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrdersAsSeller(GetCompletedOrdersInputDto input);

        Task<GetEachTypeOrderCountOutputDto> GetEachTypeOrderCountAsBuyer();

        Task<GetEachTypeOrderCountOutputDto> GetEachTypeOrderCountAsSeller();
        /// <summary>
        /// 获取已完成的所有订单（用户可以是卖家也可以是买家）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GetCompletedOrdersOutputDto>> GetCompletedOrders(GetCompletedOrdersInputDto input);
    }
}