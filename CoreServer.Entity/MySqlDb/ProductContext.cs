using System;
using CoreServer.Common.Configuration;
using SqlSugar;

namespace CoreServer.Entity.MySqlDb
{
    public class ProductContext : BaseDbContext
    {
        public ProductContext(ConnectionConfig connectionConfig) : base(connectionConfig)
        {
        }

        public static void Execute(Action<ISqlSugarClient> action)
        {
            var productContext = new ProductContext(new ConnectionConfig
            {
                DbType = DbType.MySql,
                ConnectionString = AppConfig.ConnectionString,
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.SystemTable
            });
            productContext._Execute(action);
        }

        public static T Execute<T>(Func<ISqlSugarClient, T> func)
        {
            var productContext = new ProductContext(new ConnectionConfig
            {
                DbType = DbType.MySql,
                ConnectionString = AppConfig.ConnectionString,
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.SystemTable
            });
            return productContext._Execute(func);
        }
    }
}