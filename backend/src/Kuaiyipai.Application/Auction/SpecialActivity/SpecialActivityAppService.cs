using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Castle.Core.Internal;
using Kuaiyipai.Auction.SpecialActivity.Dto;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.Auction.SpecialActivity
{
    public class SpecialActivityAppService : KuaiyipaiAppServiceBase, ISpecialActivityAppService
    {
        private readonly IRepository<Entities.SpecialActivity, Guid> _specialActivityRepository;

        public SpecialActivityAppService(IRepository<Entities.SpecialActivity, Guid> specialActivityRepository)
        {
            _specialActivityRepository = specialActivityRepository;
        }

        public async Task<PagedResultDto<GetSpecialActivitiesOutputDto>> GetSpecialActivities(GetSpecialActivitiesInputDto input)
        {
            var query = _specialActivityRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(s => new GetSpecialActivitiesOutputDto
            {
                Id = s.Id,
                InvitationCode = s.InvitationCode,
                Name = s.Name,
                Remarks = s.Remarks,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                CoverUrl = s.CoverUrl
            }).ToListAsync();

            return new PagedResultDto<GetSpecialActivitiesOutputDto>(count, list);
        }
    }
}