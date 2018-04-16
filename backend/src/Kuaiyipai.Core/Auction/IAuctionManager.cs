using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Kuaiyipai.Auction.Entities;

namespace Kuaiyipai.Auction
{
    public interface IAuctionManager : IDomainService
    {
        Task CompleteAuctionAndGenerateOrder();
    }
}