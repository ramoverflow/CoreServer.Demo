using CoreServer.Common.Configuration.ConfigModel;
using Microsoft.Extensions.Configuration;

namespace CoreServer.Common.Configuration
{
    public static class AppConfig
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    var databases = Configuration.GetSection("Databases")
                        .Get<Database[]>();

                    _connectionString = databases[0].ConnectionString;
                }
                
                return _connectionString;
            }
        }

        public static IConfiguration Configuration { private get; set; }
    }
}