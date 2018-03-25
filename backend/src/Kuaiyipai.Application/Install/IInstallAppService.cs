using System.Threading.Tasks;
using Abp.Application.Services;
using Kuaiyipai.Install.Dto;

namespace Kuaiyipai.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}