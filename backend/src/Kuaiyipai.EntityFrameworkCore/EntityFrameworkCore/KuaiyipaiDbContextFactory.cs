using Kuaiyipai.Configuration;
using Kuaiyipai.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kuaiyipai.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class KuaiyipaiDbContextFactory : IDesignTimeDbContextFactory<KuaiyipaiDbContext>
    {
        public KuaiyipaiDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<KuaiyipaiDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            KuaiyipaiDbContextConfigurer.Configure(builder, configuration.GetConnectionString(KuaiyipaiConsts.ConnectionStringName));

            return new KuaiyipaiDbContext(builder.Options);
        }
    }
}