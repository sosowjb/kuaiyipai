using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kuaiyipai.Authorization.Permissions.Dto;

namespace Kuaiyipai.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
