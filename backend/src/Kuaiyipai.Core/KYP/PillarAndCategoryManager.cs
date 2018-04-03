using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Kuaiyipai.KYP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.KYP
{
    public class PillarAndCategoryManager : KuaiyipaiDomainServiceBase, IPillarAndCategoryManager
    {
        private readonly IRepository<Pillar> _pillarRepository;

        public PillarAndCategoryManager(IRepository<Pillar> pillarRepository)
        {
            _pillarRepository = pillarRepository;
        }

        public async Task<int> CreatePillar(Pillar pillar)
        {
            var maxCode = await _pillarRepository.GetAll().MaxAsync(p => p.Code);
            var nextCode = (Convert.ToInt32(maxCode) + 1).ToString().PadLeft(3, '0');
            pillar.Code = nextCode;
            return await _pillarRepository.InsertAndGetIdAsync(pillar);
        }
    }
}