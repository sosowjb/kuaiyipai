using System;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace Kuaiyipai.Auction.Order
{
    public interface IOrderOpsAppService : IApplicationService
    {
        Task Pay(Guid orderId);
    }
}