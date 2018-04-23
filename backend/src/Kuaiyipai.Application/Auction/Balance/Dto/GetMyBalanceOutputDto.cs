namespace Kuaiyipai.Auction.Balance.Dto
{
    public class GetMyBalanceOutputDto
    {
        public double Total { get; set; }

        public double Frozen { get; set; }

        public double Available { get; set; }
    }
}