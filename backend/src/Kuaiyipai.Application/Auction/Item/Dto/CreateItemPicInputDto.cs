using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class CreateItemPicInputDto : EntityDto<Guid>
    {
        public bool IsCover { get; set; }
    }
}