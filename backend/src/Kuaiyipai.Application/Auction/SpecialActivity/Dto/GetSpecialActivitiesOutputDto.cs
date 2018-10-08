using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.SpecialActivity.Dto
{
    public class GetSpecialActivitiesOutputDto : EntityDto<Guid>
    {
        public string InvitationCode { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string CoverUrl { get; set; }
    }
}