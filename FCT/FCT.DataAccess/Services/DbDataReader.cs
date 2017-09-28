using System.Collections.Generic;
using FCT.Infrastructure.Models;
using FCT.Infrastructure.Services;
using System.Data;
using Dapper;
using System.Linq;
using FCT.DataAccess.Utilities;

namespace FCT.DataAccess.Services
{
    public class DbDataReader : IDbDataReader
    {
        public List<CarDescription> GetCarDescriptions()
        {
            var carDescriptions = new List<CarDescription>();
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                carDescriptions = connection.Query<CarDescription>("dbo.spGetAllCarDesciptions").ToList();
            }
            return carDescriptions;
        }

        public List<FuelConEntry> GetFuelConEntries()
        {
            var fuelConEntries = new List<FuelConEntry>();
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                fuelConEntries = connection.Query<FuelConEntry>("dbo.spGetAllFuelConsumption").ToList();
            }
            return fuelConEntries;
        }

        public List<FuelConEntry> GetFuelConEntries(int carId)
        {
            var fuelConEntries = new List<FuelConEntry>();
            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                fuelConEntries = connection.Query<FuelConEntry>("dbo.spGetFuelConsumptionByCarId @CarId", new {CarId = carId}).ToList();
            }
            return fuelConEntries;
        }
    }
}
