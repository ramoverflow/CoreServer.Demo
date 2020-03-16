using System;
using AutoMapper;
using CoreServer.Common;
using CoreServer.Common.Configuration;
using SqlSugar;

namespace CoreServer.Service
{
    public abstract class BaseService
    {
        private readonly ConnectionConfig _connectionConfig;

        protected IMapper MapUtil { get; }

        protected BaseService(Action<IMapperConfigurationExpression> mapperConfig = null)
        {
            _connectionConfig = new ConnectionConfig
            {
                DbType = DbType.MySql,
                ConnectionString = AppConfig.ConnectionString,
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.SystemTable
            };

            if (mapperConfig != null)
                MapUtil = new Mapper(new MapperConfiguration(mapperConfig));
        }

        public void Execute(Action<ISqlSugarClient> action)
        {
            using (var dbClient = new SqlSugarClient(_connectionConfig))
            {
                action(dbClient);
            }
        }

        public T Execute<T>(Func<ISqlSugarClient, T> func)
        {
            using (var dbClient = new SqlSugarClient(_connectionConfig))
            {
                return func(dbClient);
            }
        }
    }
}