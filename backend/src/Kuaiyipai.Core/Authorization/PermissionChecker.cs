using Abp.Authorization;
using Kuaiyipai.Authorization.Roles;
using Kuaiyipai.Authorization.Users;

namespace Kuaiyipai.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
