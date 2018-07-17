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

        Task EditItem(EditItemInputDto input);

        Task<PagedResultDto<GetMyDraftingItemsOutputDto>> GetMyDraftingItems(GetMyDraftingItemsInputDto input);

        Task<PagedResultDto<GetMyAuctionItemsOutputDto>> GetMyAuctionItems(GetMyAuctionItemsInputDto input);

        Task<PagedResultDto<GetMyCompletedItemsOutputDto>> GetMyCompletedItems(GetMyCompletedItemsInputDto input);

        Task<PagedResultDto<GetMyTerminatedItemsOutputDto>> GetMyTerminatedItems(GetMyTerminatedItemsInputDto input);

        Task<GetDraftingItemOutputDto> GetDraftingItem(EntityDto<Guid> input);

        Task<GetAuctionItemOutputDto> GetAuctionItem(EntityDto<Guid> input);

        Task<GetCompletedItemOutputDto> GetCompletedItem(EntityDto<Guid> input);

        Task<GetTerminatedItemOutputDto> GetTerminatedItem(EntityDto<Guid> input);

        Task<GetItemOutputDto> GetItem(EntityDto<Guid> input);
        
        Task<PagedResultDto<GetAuctionItemsOutputDto>> GetAuctionItems(GetAuctionItemsInputDto input);

        Task<PagedResultDto<GetAuctionItemsOutputDto>> GetAuctionItemsEx(GetAuctionItemsExInputDto input);

        Task<UploadPictureOutputDto> UploadPicture();

        Task<ListResultDto<GetItemPicturesOutputDto>> GetItemPictures(GetItemPicturesInputDto input);

        Task<GetItemPicturesOutputDto> GetCoverPicture(GetItemPicturesInputDto input);

        Task DeleteItemPicture(EntityDto<Guid> input);
    }
}