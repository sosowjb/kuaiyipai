using System.Collections.Generic;
using Kuaiyipai.Authorization.Permissions.Dto;

namespace Kuaiyipai.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}