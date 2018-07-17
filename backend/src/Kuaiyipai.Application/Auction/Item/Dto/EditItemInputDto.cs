﻿using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kuaiyipai.Auction.Item.Dto
{
    public class EditItemInputDto : EntityDto<Guid>
    {
        [Required]
        public int PillarId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 起步价
        /// </summary>
        [Required]
        public double StartPrice { get; set; }

        /// <summary>
        /// 加价幅度
        /// </summary>
        [Required]
        public double StepPrice { get; set; }

        /// <summary>
        /// 开始拍卖时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束拍卖时间
        /// </summary>
        public DateTime Deadline { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public List<CreateItemPicInputDto> PictureList { get; set; }

        public EditItemInputDto()
        {
            PictureList = new EditableList<CreateItemPicInputDto>();
        }
    }
}
