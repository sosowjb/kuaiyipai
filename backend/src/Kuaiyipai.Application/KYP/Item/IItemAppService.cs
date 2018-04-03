using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.KYP.Item.Dto;

namespace Kuaiyipai.KYP.Item
{
    public interface IItemAppService : IApplicationService
    {
        Task SaveItemToDraft(SaveItemToDraftInputDto input);
    }
}