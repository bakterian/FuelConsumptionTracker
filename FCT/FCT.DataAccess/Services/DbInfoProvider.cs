using FCT.Infrastructure.Interfaces;
using System.Data;
using FCT.DataAccess.Utilities;

namespace FCT.DataAccess.Services
{
    public class DbInfoProvider : IDbInfoProvider
    {
        public string GetConnectionStatusInfos()
        {
            var statusInfo = "Failed To acquires status data";
            using (IDbConnection connection =new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
            {
                statusInfo = $"Connection string used: {connection.ConnectionString}\r\nDatabase name: {connection.Database}";
            }
            return statusInfo;
        }
    }
}
