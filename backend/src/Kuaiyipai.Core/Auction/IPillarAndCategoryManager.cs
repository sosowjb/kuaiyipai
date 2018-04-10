using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Kuaiyipai.Auction
{
    public interface IPillarAndCategoryManager : IDomainService
    {
        Task<int> CreatePillar(string pillarName);

        Task UpdatePillar(int pillarId, string pillarName);

        Task DeletePillar(int pillarId, bool recursive);

        Task<int> CreateCategory(string categoryName, int pillarId);

        Task UpdateCategory(int categoryId, string categoryName);

        Task MoveCategory(int categoryId, int pillarId);

        Task DeleteCategory(int categoryId);
    }
}