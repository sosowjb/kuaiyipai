namespace Kuaiyipai.KYP.Entities
{
    public enum OrderStatus
    {
        /// <summary>
        /// 待付款
        /// </summary>
        WaitingForPayment = 1,
        /// <summary>
        /// 待发货
        /// </summary>
        WaitingForSending = 2,
        /// <summary>
        /// 待收货
        /// </summary>
        WaitingForReceiving = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 4
    }
}