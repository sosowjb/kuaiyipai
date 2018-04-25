using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class GetItemPicturesOutputDto : EntityDto<Guid>
    {
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsCover { get; set; }
    }
}