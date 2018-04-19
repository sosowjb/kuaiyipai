using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Quartz;
using Kuaiyipai.Auction;
using Quartz;

namespace Kuaiyipai.Web.Startup
{
    public class OrderGenerator : JobBase, ISingletonDependency
    {
        private readonly IAuctionManager _auctionManager;

        public OrderGenerator(IAuctionManager auctionManager)
        {
            _auctionManager = auctionManager;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            await _auctionManager.CompleteAuctionAndGenerateOrder();
        }
    }
}