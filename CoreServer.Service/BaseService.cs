using System;
using AutoMapper;

namespace CoreServer.Service
{
    public abstract class BaseService
    {
        protected IMapper MapUtil { get; }

        protected BaseService(Action<IMapperConfigurationExpression> mapperConfig = null)
        {
            if (mapperConfig != null)
                MapUtil = new Mapper(new MapperConfiguration(mapperConfig));
        }
    }
}