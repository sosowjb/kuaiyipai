using System.Data.Common;
using Abp.Extensions;
using MySql.Data.MySqlClient;

namespace Kuaiyipai.EntityFrameworkCore
{
    public static class DatabaseCheckHelper
    {
        public static bool Exist(string connectionString)
        {
            if (connectionString.IsNullOrEmpty())
            {
                //connectionString is null for unit tests
                return true;
            }

            using (DbConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
    }
}
