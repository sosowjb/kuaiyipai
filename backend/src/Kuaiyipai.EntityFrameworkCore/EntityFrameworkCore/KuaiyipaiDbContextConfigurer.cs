using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.EntityFrameworkCore
{
    public static class KuaiyipaiDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<KuaiyipaiDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<KuaiyipaiDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection);
        }
    }
}