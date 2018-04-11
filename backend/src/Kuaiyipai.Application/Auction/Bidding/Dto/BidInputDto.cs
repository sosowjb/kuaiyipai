using System;
using System.ComponentModel.DataAnnotations;

namespace Kuaiyipai.Auction.Bidding.Dto
{
    public class BidInputDto
    {
        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public double Price { get; set; }
    }
}