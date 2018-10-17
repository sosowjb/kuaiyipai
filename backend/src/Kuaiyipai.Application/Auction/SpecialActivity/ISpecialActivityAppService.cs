using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Auction.SpecialActivity.Dto;

namespace Kuaiyipai.Auction.SpecialActivity
{
    public interface ISpecialActivityAppService : IApplicationService
    {
        Task<PagedResultDto<GetSpecialActivityOutputDto>> GetSpecialActivities(GetSpecialActivitiesInputDto input);

        Task<GetSpecialActivityOutputDto> GetSpecialActivity(Guid id);
    }
}