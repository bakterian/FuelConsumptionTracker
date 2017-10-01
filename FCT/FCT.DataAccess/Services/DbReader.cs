using System.Collections.Generic;
using FCT.Infrastructure.Models;
using FCT.Infrastructure.Services;
using System.Data;
using Dapper;
using System.Linq;
using FCT.DataAccess.Utilities;

namespace FCT.DataAccess.Services
{
    public class DbReader : IDbReader
    {
        public IEnumerable<CarDescription> GetCarDescriptions()
        {
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                return connection.Query<CarDescription>("dbo.spGetAllCarDesciptions").ToList();
            }
        }

        public IEnumerable<FuelConEntry> GetFuelConEntries()
        {
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                return connection.Query<FuelConEntry>("dbo.spGetAllFuelConsumption").ToList();
            }
        }

        public IEnumerable<FuelConEntry> GetFuelConEntries(int carId)
        {
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                return connection.Query<FuelConEntry>("dbo.spGetFuelConsumptionByCarId @CarId", new {CarId = carId}).ToList();
            }
        }
    }
}
