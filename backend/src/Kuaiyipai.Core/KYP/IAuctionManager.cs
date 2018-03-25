using System;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Kuaiyipai.KYP
{
    public interface IAuctionManager : IDomainService
    {
        Task CreateBid(Guid itemId, double price);
    }
}