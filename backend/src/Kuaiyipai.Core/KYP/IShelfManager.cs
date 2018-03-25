using System;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Kuaiyipai.KYP
{
    public interface IShelfManager : IDomainService
    {
        Task AddItemForAuction(string title, string desc, double startPrice, double step, DateTime deadline);

        Task UpdateItemInfo(Guid id, string title, string desc, double startPrice, double step, DateTime deadline);

        Task ReAddItemForAuction(Guid id);

        Task DeleteItemFromShelf(Guid id);

        Task<ItemStatus> GetStatus(Guid id);

        Task<Item> GetItemInfo(Guid id);

        Task<Item> GetItemInfo(string code);
    }
}