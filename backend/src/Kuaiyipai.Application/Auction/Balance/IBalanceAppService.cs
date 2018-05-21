using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.Balance.Dto;

namespace Kuaiyipai.Auction.Balance
{
    public interface IBalanceAppService : IApplicationService
    {
        Task<GetMyBalanceOutputDto> GetMyBalance();

        Task<PagedResultDto<GetMyBalanceRecordsOutputDto>> GetMyBalanceRecords(GetMyBalanceRecordsInputDto input);

        Task Charge(ChargeInputDto input);

        Task Withdraw(WithdrawInputDto input);
    }
}