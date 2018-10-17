using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
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

        public async Task<PagedResultDto<GetSpecialActivityOutputDto>> GetSpecialActivities(GetSpecialActivitiesInputDto input)
        {
            var query = _specialActivityRepository.GetAll();

            if (!input.Sorting.IsNullOrEmpty())
            {
                query = query.OrderBy(input.Sorting);
            }

            var count = await query.CountAsync();
            var list = await query.PageBy(input).Select(s => new GetSpecialActivityOutputDto
            {
                Id = s.Id,
                InvitationCode = s.InvitationCode,
                Name = s.Name,
                Remarks = s.Remarks,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                CoverUrl = s.CoverUrl
            }).ToListAsync();

            return new PagedResultDto<GetSpecialActivityOutputDto>(count, list);
        }

        public async Task<GetSpecialActivityOutputDto> GetSpecialActivity(Guid id)
        {
            var act = await _specialActivityRepository.FirstOrDefaultAsync(id);
            if (act == null)
            {
                throw new UserFriendlyException("专场不存在");
            }

            return new GetSpecialActivityOutputDto
            {
                Id = act.Id,
                InvitationCode = act.InvitationCode,
                Name = act.Name,
                Remarks = act.Remarks,
                StartTime = act.StartTime,
                EndTime = act.EndTime,
                CoverUrl = act.CoverUrl
            };
        }
    }
}