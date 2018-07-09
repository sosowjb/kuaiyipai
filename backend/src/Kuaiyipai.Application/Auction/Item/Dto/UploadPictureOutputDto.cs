using System;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class UploadPictureOutputDto : EntityDto<Guid>
    {
       public string Url { get; set; }
    }
}