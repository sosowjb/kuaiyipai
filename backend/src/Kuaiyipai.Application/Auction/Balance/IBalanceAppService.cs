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

        Task<string> Charge(ChargeInputDto input);

        Task<string> CompleteCharge();

        Task Withdraw(WithdrawInputDto input);
    }
}