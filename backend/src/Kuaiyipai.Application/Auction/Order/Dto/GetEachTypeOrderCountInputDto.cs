using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class GetEachTypeOrderCountInputDto
    {
        /// <summary>
        /// 登陆用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 登陆用户类型
        /// </summary>
        public int UserType { get; set; }
    }
}