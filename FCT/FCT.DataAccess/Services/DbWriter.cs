﻿using FCT.Infrastructure.Interfaces;
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
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
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
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
            {
                affectedRows = connection.Execute("dbo.spUpdateCarDescription @Id, @Description, @Manufacturer,	@Model," +
                    " @HorsePower, @EngineSize, @PetrolType, @FuelTankSize, @Weight, @TopSpeed," +
                    " @Acceleration, @AvgFuelConsumption, @ProductionYear", carDescriptions);
            }

            return affectedRows;
        }

        public int DeleteCarDescriptions(IEnumerable<CarDescription> carDescriptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
            {
                affectedRows = connection.Execute("dbo.spDeleteCarDescription @Id", carDescriptions);
            }

            return affectedRows;
        }


        public int InsertFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
            {
                affectedRows = connection.Execute("dbo.spInsertFuelConsumption @CarId, @CarDescription, @PetrolStationDesc,	@PetrolType," +
                    " @FuelingDate, @LiterAmount, @PricePerLiter, @FullPrice, @DistanceMade," +
                    " @FuelConsumption, @Terrain", fuelConsumptions);
            }

            return affectedRows;
        }

        public int UpdateFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
            {
                affectedRows = connection.Execute("dbo.spUpdateFuelConsumption @Id, @CarId, @CarDescription, @PetrolStationDesc, @PetrolType," +
                    " @FuelingDate, @LiterAmount, @PricePerLiter, @FullPrice, @DistanceMade," +
                    " @FuelConsumption, @Terrain", fuelConsumptions);
            }

            return affectedRows;
        }

        public int DeleteFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions)
        {
            var affectedRows = 0;

            using (IDbConnection connection =
                new System.Data.SqlClient.SqlConnection(ConnectionHepler.ConVal()))
            {
                affectedRows = connection.Execute("dbo.spDeleteFuelConsumption @Id", fuelConsumptions);
            }

            return affectedRows;
        }
    }
}
