using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Bidding.Dto;

namespace Kuaiyipai.Auction.Bidding
{
    public interface IBiddingAppService : IApplicationService
    {
        Task<Guid> Bid(BidInputDto input);

        Task<PagedResultDto<GetBiddingsOutputDto>> GetBiddings(GetBiddingsInputDto input);
    }
}