using Microsoft.Extensions.Configuration;

namespace Kuaiyipai.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
