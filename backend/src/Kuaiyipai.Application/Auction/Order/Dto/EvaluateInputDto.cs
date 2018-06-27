using System;

namespace Kuaiyipai.Auction.Order.Dto
{
    public class EvaluateInputDto
    {
        public Guid OrderId { get; set; }

        public int EvaluationLevel { get; set; }

        public string EvaluationContent { get; set; }
    }
}