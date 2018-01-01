using System.Configuration;

namespace FCT.DataAccess.Utilities
{
    public static class ConnectionHepler
    {
        public static string ConVal()
        {
            var connectionStrName = ConfigurationManager.AppSettings.Get("DbConnectionName");
            return ConfigurationManager.ConnectionStrings[connectionStrName].ConnectionString;
        }
    }
}
 