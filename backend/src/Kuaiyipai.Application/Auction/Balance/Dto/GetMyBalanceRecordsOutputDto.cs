using System;
using Abp.Application.Services.Dto;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Auction.Balance.Dto
{
    public class GetMyBalanceRecordsOutputDto : EntityDto<Guid>
    {
        public long UserId { get; set; }

        public string UserFullName { get; set; }

        public double Amount { get; set; }

        public DateTime RecordTime { get; set; }

        public string Remarks { get; set; }
    }
}