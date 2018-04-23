using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Item.Dto;

namespace Kuaiyipai.Auction.Item
{
    public interface IItemAppService : IApplicationService
    {
        Task<Guid> CreateItem(CreateItemInputDto input);

        Task StartAuction(EntityDto<Guid> input);

        Task CompleteAuction(EntityDto<Guid> input);

        Task TerminateAuction(EntityDto<Guid> input);

        Task RecreateItem(EntityDto<Guid> input);

        Task DeleteItem(EntityDto<Guid> input);

        Task<PagedResultDto<GetMyDraftingItemsOutputDto>> GetMyDraftingItems(GetMyDraftingItemsInputDto input);

        Task<PagedResultDto<GetMyAuctionItemsOutputDto>> GetMyAuctionItems(GetMyAuctionItemsInputDto input);

        Task<PagedResultDto<GetMyCompletedItemsOutputDto>> GetMyCompletedItems(GetMyCompletedItemsInputDto input);

        Task<PagedResultDto<GetMyTerminatedItemsOutputDto>> GetMyTerminatedItems(GetMyTerminatedItemsInputDto input);

        /// <summary>
        /// 获取正在拍卖的商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GetAuctionItemsOutputDto>> GetAuctionItems(GetAuctionItemsInputDto input);
    }
}