using System;
using SqlSugar;

namespace CoreServer.Entity
{
    public abstract class BaseDbContext
    {
        private readonly ConnectionConfig _connectionConfig;

        protected BaseDbContext(ConnectionConfig connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        protected void _Execute(Action<ISqlSugarClient> action)
        {
            using (var dbClient = new SqlSugarClient(_connectionConfig))
            {
                action(dbClient);
            }
        }

        protected T _Execute<T>(Func<ISqlSugarClient, T> func)
        {
            using (var dbClient=new SqlSugarClient(_connectionConfig))
            {
                return func(dbClient);
            }
        }
    }
}