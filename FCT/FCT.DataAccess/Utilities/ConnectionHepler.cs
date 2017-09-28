using System.Configuration;

namespace FCT.DataAccess.Utilities
{
    public static class ConnectionHepler
    {
        public static string ConVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
 