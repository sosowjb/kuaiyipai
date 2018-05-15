using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Kuaiyipai.Auction.Pillar.Dto
{
    public class GetPillarsOutputDto : EntityDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public IList<Entities.Category> categories { get; set; }
    }
}