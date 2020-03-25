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

        private static CapConfig _capConfig;

        public static CapConfig CapConfig
        {
            get
            {
                if (_capConfig == null)
                {
                    _capConfig = Configuration.GetSection("CapConfig").Get<CapConfig>();
                }

                return _capConfig;
            }
        }

        #region config上下文

        public static IConfiguration Configuration { private get; set; }

        #endregion
    }
}