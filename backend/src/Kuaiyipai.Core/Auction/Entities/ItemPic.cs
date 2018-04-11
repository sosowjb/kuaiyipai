using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_ItemPictures")]
    public class ItemPic : Entity<Guid>
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        public string Extension { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Width { get; set; }
    }
}