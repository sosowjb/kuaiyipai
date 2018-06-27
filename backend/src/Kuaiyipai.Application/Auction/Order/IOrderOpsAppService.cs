using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.Auction.Order.Dto;

namespace Kuaiyipai.Auction.Order
{
    public interface IOrderOpsAppService : IApplicationService
    {
        Task Pay(PayInputDto input);

        Task Send(SendInputDto input);

        Task Receive(ReceiveInputDto input);

        Task Evaluate(EvaluateInputDto input);
    }
}