using System.Threading.Tasks;
using Abp.Domain.Services;
using Kuaiyipai.KYP.Entities;

namespace Kuaiyipai.KYP
{
    public interface IAuctionManager : IDomainService
    {
        /// <summary>
        /// 获取最高出价
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<Bid> GetHighestBid(long itemId);

        Task OfferPrice(long itemId, double price);
    }
}