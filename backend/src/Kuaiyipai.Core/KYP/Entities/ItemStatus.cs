namespace Kuaiyipai.KYP.Entities
{
    public enum ItemStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Drafting = 1,
        /// <summary>
        /// 拍卖中
        /// </summary>
        Auction = 2,
        /// <summary>
        /// 已截拍
        /// </summary>
        Completed = 3,
        /// <summary>
        /// 已流拍
        /// </summary>
        Terminated = 4
    }
}