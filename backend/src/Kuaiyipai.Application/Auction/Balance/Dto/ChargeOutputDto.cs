namespace Kuaiyipai.Auction.Balance.Dto
{
    public class ChargeOutputDto
    {
        public string PrepayId { get; set; }

        public string Sign { get; set; }

        public string NonceStr { get; set; }

        public long TimeStamp { get; set; }
    }
}