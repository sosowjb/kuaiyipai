using Abp.AspNetZeroCore;
using Abp.AspNetZeroCore.Web.Authentication.External;
using Abp.AspNetZeroCore.Web.Authentication.External.Facebook;
using Abp.AspNetZeroCore.Web.Authentication.External.Google;
using Abp.AspNetZeroCore.Web.Authentication.External.Microsoft;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Quartz.Configuration;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Kuaiyipai.Configuration;
using Kuaiyipai.EntityFrameworkCore;
using Kuaiyipai.MultiTenancy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Kuaiyipai.Web.Startup
{
    [DependsOn(
        typeof(KuaiyipaiWebCoreModule)
    )]
    public class KuaiyipaiWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public KuaiyipaiWebHostModule(
            IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:ServerRootAddress"] ?? "http://localhost:22742/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(KuaiyipaiWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!DatabaseCheckHelper.Exist(_appConfiguration["ConnectionStrings:Default"]))
            {
                return;
            }

            if (IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
                workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());
            }

            // create job
            IScheduler scheduler = Configuration.Modules.AbpQuartz().Scheduler;
            var job = scheduler.GetJobDetail(new JobKey("OrderGenerator", "Order")).GetAwaiter().GetResult();
            if (job == null)
            {
                job = JobBuilder.Create<OrderGenerator>()
                    .WithIdentity("OrderGeneratorJob", "Order")
                    .WithDescription("OrderGenerator")
                    .StoreDurably(true)
                    .Build();

                scheduler.AddJob(job, true);

                var trigger = TriggerBuilder.Create()
                    .WithIdentity("OrderGeneratorTrigger", "Order")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(60)
                        .RepeatForever())
                    .ForJob(job)
                    .Build();

                scheduler.ScheduleJob(trigger);
            }
        }
    }
}
