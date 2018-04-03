using System.Threading.Tasks;
using Abp.Domain.Services;
using Kuaiyipai.KYP.Entities;

namespace Kuaiyipai.KYP
{
    public interface IPillarAndCategoryManager : IDomainService
    {
        Task<int> CreatePillar(Pillar pillar);
    }
}