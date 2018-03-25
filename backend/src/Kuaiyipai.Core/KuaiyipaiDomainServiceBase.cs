using Abp.Domain.Services;

namespace Kuaiyipai
{
    public abstract class KuaiyipaiDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected KuaiyipaiDomainServiceBase()
        {
            LocalizationSourceName = KuaiyipaiConsts.LocalizationSourceName;
        }
    }
}
