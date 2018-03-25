﻿using System.Threading.Tasks;
using Abp.Application.Services;

namespace Kuaiyipai.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task UpgradeTenantToEquivalentEdition(int upgradeEditionId);
    }
}
