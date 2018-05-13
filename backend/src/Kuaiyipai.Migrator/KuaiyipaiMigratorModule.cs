using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Kuaiyipai.Configuration;
using Kuaiyipai.EntityFrameworkCore;
using Kuaiyipai.Migrator.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Kuaiyipai.Migrator
{
    [DependsOn(typeof(KuaiyipaiEntityFrameworkCoreModule))]
    public class KuaiyipaiMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public KuaiyipaiMigratorModule(KuaiyipaiEntityFrameworkCoreModule kuaiyipaiEntityFrameworkCoreModule)
        {
            kuaiyipaiEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(KuaiyipaiMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                KuaiyipaiConsts.ConnectionStringName
                );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(KuaiyipaiMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}