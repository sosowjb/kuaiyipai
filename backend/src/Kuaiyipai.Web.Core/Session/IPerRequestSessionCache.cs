using System.Threading.Tasks;
using Kuaiyipai.Sessions.Dto;

namespace Kuaiyipai.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
