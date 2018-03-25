using System.Threading.Tasks;

namespace Kuaiyipai.Identity
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}