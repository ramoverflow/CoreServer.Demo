using System;

namespace CoreServer.Common.Core
{
    public class AutoServiceAttribute:System.Attribute
    {
        public Type Interface { get; }

        public AutoServiceAttribute(Type @interface)
        {
            Interface = @interface;
        }
    }
}