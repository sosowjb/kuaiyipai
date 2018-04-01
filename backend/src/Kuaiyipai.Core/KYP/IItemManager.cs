using System.Threading.Tasks;
using Abp.Domain.Services;
using Kuaiyipai.KYP.Entities;

namespace Kuaiyipai.KYP
{
    public interface IItemManager : IDomainService
    {
        /// <summary>
        /// 保存到草稿
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task SaveItemToDraft(Item item);

        /// <summary>
        /// 保存并发布
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task SaveItemAndPublish(Item item);

        /// <summary>
        /// 开始拍卖
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task StartAuction(long itemId);

        /// <summary>
        /// 商品成交 - 截拍
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task Bought(long itemId);

        /// <summary>
        /// 无人出价 - 流拍
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task NoOneBuy(long itemId);

        /// <summary>
        /// 重新开始拍卖
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task RestartAuction(long itemId);

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task EditItem(Item item);

        /// <summary>
        /// 终止拍卖并下架商品
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task UnshelfItem(long itemId);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task DeleteItem(long itemId);
    }
}