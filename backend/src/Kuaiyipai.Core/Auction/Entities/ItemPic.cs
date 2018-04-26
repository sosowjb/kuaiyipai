using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Kuaiyipai.Auction.Entities
{
    [Table("AUC_ItemPictures")]
    public class ItemPic : Entity<Guid>
    {
        public Guid ItemId { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public long Size { get; set; }

        public string Extension { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public bool IsCover { get; set; }

        public int Index { get; set; }
    }
}