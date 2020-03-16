using System;

namespace CoreServer.Common
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