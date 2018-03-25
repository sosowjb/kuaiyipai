using Abp.Application.Services;
using Kuaiyipai.Dto;
using Kuaiyipai.Logging.Dto;

namespace Kuaiyipai.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
