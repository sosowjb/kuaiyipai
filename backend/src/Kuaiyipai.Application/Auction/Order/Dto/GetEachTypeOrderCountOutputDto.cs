using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class GetEachTypeOrderCountOutputDto
    {
        /// <summary>
        /// 待支付
        /// </summary>
        public int WaitPay { get; set; }
        /// <summary>
        /// 待发货
        /// </summary>
        public int WaitSend { get; set; }
        /// <summary>
        /// 待收货
        /// </summary>
        public int WaitReceive { get; set; }
    }
}