using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kuaiyipai.MultiTenancy.HostDashboard.Dto;

namespace Kuaiyipai.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}