using FCT.Control.Services;
using FCT.Infrastructure.Attributes;
using FCT.Infrastructure.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace FCT.Control.UnitTests
{
    public class DataTableMapperUnitTests
    {
        [Fact]
        public void ShouldConvertFuelConEntriesToDataTable()
        {
            var fuelConEntry1 = new FuelConEntry()
            {
                CarId = 1,
                PetrolStationDesc = "shell",
                PetrolType = "gasoline",
                FuelingDate = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"),
                LiterAmount = 100.0M,
                PricePerLiter = 4.5M,
                FullPrice = 450.0M,
                DistanceMade = 1000.0M,
                FuelConsumption = 10.0M,
                Terrain = "highways"
            };
            var fuelConEntry2 = new FuelConEntry()
            {
                CarId = 1,
                PetrolStationDesc = "bp",
                PetrolType = "gasoline",
                FuelingDate = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"),
                LiterAmount = 200.0M,
                PricePerLiter = 4.5M,
                FullPrice = 450.0M,
                DistanceMade = 1000.0M,
                FuelConsumption = 20.0M,
                Terrain = "city"
            };

            var fuelConsumptions = new List<FuelConEntry>() { fuelConEntry1, fuelConEntry2 };

            var mapper = new DataTableMapper();

            var dataTable = mapper.ConvertToDataTable(fuelConsumptions, new Attribute[] { new PresentableItem() });

            Assert.NotNull(dataTable);
            Assert.Equal(10, dataTable.Columns.Count);
            Assert.Equal(2, dataTable.Rows.Count);
            Assert.Equal(fuelConEntry1.LiterAmount, dataTable.Rows[0][4]);
            Assert.Equal(fuelConEntry1.Terrain, dataTable.Rows[0][9]);
            Assert.Equal(fuelConEntry2.LiterAmount, dataTable.Rows[1][4]);
            Assert.Equal(fuelConEntry2.Terrain, dataTable.Rows[1][9]);
        }

        [Fact]
        public void ShouldConvertCarDescriptionsToDataTable()
        {
            var carDesc1 = new CarDescription()
            {
                Description = "TestCar1",
                Manufacturer = "Mazda",
                Model = "RX8",
                HorsePower = 251,
                EngineSize = 1308,
                PetrolType = "gasoline",
                FuelTankSize = 60,
                Weight = 1330,
                TopSpeed = 240.0M,
                Acceleration = 6.4M,
                AvgFuelConsumption = 11.4M,
                ProductionYear = 2009
            };
            var carDesc2 = new CarDescription()
            {
                Description = "TestCar2",
                Manufacturer = "Honda",
                Model = "ASX",
                HorsePower = 320,
                EngineSize = 2800,
                PetrolType = "gasoline",
                FuelTankSize = 60,
                Weight = 900,
                TopSpeed = 270.0M,
                Acceleration = 5.4M,
                AvgFuelConsumption = 15.4M,
                ProductionYear = 2007
            };

            var carDescriptions = new List<CarDescription>() { carDesc1, carDesc2 };
            var mapper = new DataTableMapper();
            var dataTable = mapper.ConvertToDataTable(carDescriptions, new Attribute[] { new PresentableItem() });

            Assert.NotNull(dataTable);
            Assert.Equal(12, dataTable.Columns.Count);
            Assert.Equal(2, dataTable.Rows.Count);
            Assert.Equal(carDesc1.Manufacturer, dataTable.Rows[0][1]);
            Assert.Equal(carDesc1.Weight, dataTable.Rows[0][7]);
            Assert.Equal(carDesc2.Manufacturer, dataTable.Rows[1][1]);
            Assert.Equal(carDesc2.Weight, dataTable.Rows[1][7]);
        }
    }
}
