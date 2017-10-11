using FCT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using FCT.Infrastructure.Models;
using System.Data;
using Dapper;
using FCT.DataAccess.Utilities;

namespace FCT.DataAccess.Services
{
    public class DbWriter : IDbWriter
    {
        public int InsertCarDescriptions(IEnumerable<CarDescription> carDescriptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection = 
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                affectedRows = connection.Execute("dbo.spInsertCarDescription @Description, @Manufacturer,	@Model," +
                    " @HorsePower, @EngineSize, @PetrolType, @FuelTankSize, @Weight, @TopSpeed," +
                    " @Acceleration, @AvgFuelConsumption, @ProductionYear", carDescriptions);
            }

            return affectedRows;
        }

        public int UpdateCarDescriptions(IEnumerable<CarDescription> carDescriptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                affectedRows = connection.Execute("dbo.spUpdateCarDescription @Id, @Description, @Manufacturer,	@Model," +
                    " @HorsePower, @EngineSize, @PetrolType, @FuelTankSize, @Weight, @TopSpeed," +
                    " @Acceleration, @AvgFuelConsumption, @ProductionYear", carDescriptions);
            }

            return affectedRows;
        }


        public int InsertFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                affectedRows = connection.Execute("dbo.spInsertFuelConsumption @CarId, @PetrolStationDesc,	@PetrolType," +
                    " @FuelingDate, @LiterAmount, @PricePerLiter, @FullPrice, @DistanceMade," +
                    " @FuelConsumption, @Terrain", fuelConsumptions);
            }

            return affectedRows;
        }

        public int UpdateFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal("CarDataDB")))
            {
                affectedRows = connection.Execute("dbo.spUpdateFuelConsumption @Id, @CarId, @PetrolStationDesc,	@PetrolType," +
                    " @FuelingDate, @LiterAmount, @PricePerLiter, @FullPrice, @DistanceMade," +
                    " @FuelConsumption, @Terrain", fuelConsumptions);
            }

            return affectedRows;
        }
    }
}
