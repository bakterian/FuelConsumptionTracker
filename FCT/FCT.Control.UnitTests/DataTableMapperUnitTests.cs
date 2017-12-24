using FCT.Control.Services;
using FCT.Infrastructure.Attributes;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Xunit;

namespace FCT.Control.UnitTests
{
    public class DataTableMapperUnitTests
    {
        private readonly ILogger _logger;

        public DataTableMapperUnitTests()
        {
            _logger = Substitute.For<ILogger>();
        }

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

            var mapper = new DataTableMapper(_logger);

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
            var mapper = new DataTableMapper(_logger);
            var dataTable = mapper.ConvertToDataTable(carDescriptions, new Attribute[] { new PresentableItem() });

            Assert.NotNull(dataTable);
            Assert.Equal(12, dataTable.Columns.Count);
            Assert.Equal(2, dataTable.Rows.Count);
            Assert.Equal(carDesc1.Manufacturer, dataTable.Rows[0][1]);
            Assert.Equal(carDesc1.Weight, dataTable.Rows[0][7]);
            Assert.Equal(carDesc2.Manufacturer, dataTable.Rows[1][1]);
            Assert.Equal(carDesc2.Weight, dataTable.Rows[1][7]);
        }

        [Fact]
        public void ShouldLogThatDataTableIsEmpty()
        {
            var testDataTable = new DataTable();

            var mapper = new DataTableMapper(_logger);

            var answer = mapper.IsConversionToEnumerablePossible<FuelConEntry>(testDataTable, new Attribute[] { new PresentableItem()});

            _logger.Received(1).Error("DataTable contains no elements.");

            Assert.False(answer);
        }

        [Fact]
        public void ShouldLogThatDataTableDoesNotContainDataRows()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("testColName1");
            testDataTable.Columns.Add("testColName2");

            var mapper = new DataTableMapper(_logger);

            var answer = mapper.IsConversionToEnumerablePossible<FuelConEntry>(testDataTable, new Attribute[] { new PresentableItem() });

            _logger.Received(1).Error("DataTable contains no elemet rows.");

            Assert.False(answer);
        }

        [Fact]
        public void ShouldNoticeMissingColumnsInFuelConsDataTable()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("CarId");
            testDataTable.Columns.Add("PetrolStationDesc");

            testDataTable.Rows.Add("0", "shell");

            var ErrorMsg = "";
            _logger.When(_ => _.Error(Arg.Any<string>())).Do(ci => ErrorMsg = ci.ArgAt<string>(0));

            var mapper = new DataTableMapper(_logger);
            var answer = mapper.IsConversionToEnumerablePossible<FuelConEntry>(testDataTable, new Attribute[] { new PresentableItem() });

            _logger.Received(1).Error(Arg.Any<string>());

            Assert.Contains("DataTable column vs target properties count mismatch", ErrorMsg);
            Assert.False(answer);
        }

        [Fact]
        public void ShouldNoticeMismatchedColumnsInCarDescDataTable()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("CarId");
            testDataTable.Columns.Add("PetrolStationDesc");
            testDataTable.Columns.Add("PetrolType");
            testDataTable.Columns.Add("Terrain");
            testDataTable.Columns.Add("LiterAmount");
            testDataTable.Columns.Add("PricePerLiter");
            testDataTable.Columns.Add("FullPrice");
            testDataTable.Columns.Add("DistanceMade");
            testDataTable.Columns.Add("FuelConsumption");
            testDataTable.Columns.Add("FuelingDate");   

            testDataTable.Rows.Add("0", "shell", "gasoline", DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"), "100.0","4.5", "450.0", "1000.0", "10.0", "highways");

            var mapper = new DataTableMapper(_logger);

            var answer = mapper.IsConversionToEnumerablePossible<FuelConEntry>(testDataTable, new Attribute[] { new PresentableItem() });

            _logger.Received(1).Error("DataTable column names are not matching target property names.");

            Assert.False(answer);
        }

        [Fact]
        public void ShouldNoticeMissingDataInFuelConsDataTable()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("CarId");
            testDataTable.Columns.Add("PetrolStationDesc");
            testDataTable.Columns.Add("PetrolType");
            testDataTable.Columns.Add("FuelingDate");
            testDataTable.Columns.Add("LiterAmount");
            testDataTable.Columns.Add("PricePerLiter");
            testDataTable.Columns.Add("FullPrice");
            testDataTable.Columns.Add("DistanceMade");
            testDataTable.Columns.Add("FuelConsumption");
            testDataTable.Columns.Add("Terrain");

            testDataTable.Rows.Add("0", "shell", "gasoline", DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"), "100.0", "4.5", "450.0", "1000.0", "10.0", "highways");
            testDataTable.Rows.Add("0", "shell", "gasoline", DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"), "100.0", "4.5", "highways");
            var mapper = new DataTableMapper(_logger);

            var answer = mapper.IsConversionToEnumerablePossible<FuelConEntry>(testDataTable, new Attribute[] { new PresentableItem() });

            _logger.Received(1).Error("DataTable row width is not consistent.");

            Assert.False(answer);
        }

        [Fact]
        public void ShouldNoticeMissingDataInCarDescDataTable()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("Description");
            testDataTable.Columns.Add("Manufacturer");
            testDataTable.Columns.Add("Model");
            testDataTable.Columns.Add("HorsePower");
            testDataTable.Columns.Add("EngineSize");
            testDataTable.Columns.Add("PetrolType");
            testDataTable.Columns.Add("FuelTankSize");
            testDataTable.Columns.Add("Weight");
            testDataTable.Columns.Add("TopSpeed");
            testDataTable.Columns.Add("Acceleration");
            testDataTable.Columns.Add("AvgFuelConsumption");
            testDataTable.Columns.Add("ProductionYear");

            testDataTable.Rows.Add("TestCar1", "Mazda", "RX8", "251", "1308", "gasoline", "60", "900", "270", "5.4","15.4","2007");
            testDataTable.Rows.Add("TestCar1", "Mazda", "RX8", "251", "5.4", "15.4", "2007");
            var mapper = new DataTableMapper(_logger);

            var answer = mapper.IsConversionToEnumerablePossible<CarDescription>(testDataTable, new Attribute[] { new PresentableItem() });

            _logger.Received(1).Error("DataTable row width is not consistent.");

            Assert.False(answer);
        }

        [Fact]
        public void ShouldConvertFuelConsDataTableToEnumerable()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("CarId");
            testDataTable.Columns.Add("PetrolStationDesc");
            testDataTable.Columns.Add("PetrolType");
            testDataTable.Columns.Add("FuelingDate");
            testDataTable.Columns.Add("LiterAmount");
            testDataTable.Columns.Add("PricePerLiter");
            testDataTable.Columns.Add("FullPrice");
            testDataTable.Columns.Add("DistanceMade");
            testDataTable.Columns.Add("FuelConsumption");
            testDataTable.Columns.Add("Terrain");

            testDataTable.Rows.Add((0).ToString(CultureInfo.CurrentCulture), "shell", "gasoline",
                                    DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"),
                                    (100.0M).ToString(), (4.5M).ToString(), (450.0M).ToString(), (1000.0M).ToString(), "10", "highways");
            testDataTable.Rows.Add((1).ToString(CultureInfo.CurrentCulture), "lukoil", "gasoline",
                                    DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss.fff"),
                                    "100", (4.5M).ToString(), "450", "1000", "20", "city");

            var mapper = new DataTableMapper(_logger);

            var fuelConEntries = mapper.ConvertToDbEnumerable<FuelConEntry>(testDataTable, new Attribute[] { new PresentableItem() }).ToArray();

            _logger.DidNotReceive().Error(Arg.Any<string>());

            Assert.Equal(2, fuelConEntries.Count());
            Assert.Equal("shell", fuelConEntries[0].PetrolStationDesc);
            Assert.Equal(10.0M, fuelConEntries[0].FuelConsumption);
            Assert.Equal("highways", fuelConEntries[0].Terrain);

            Assert.Equal("lukoil", fuelConEntries[1].PetrolStationDesc);
            Assert.Equal(20.0M, fuelConEntries[1].FuelConsumption);
            Assert.Equal("city", fuelConEntries[1].Terrain);
        }

        [Fact]
        public void ShouldConvertCarDescDataTableToEnumerable()
        {
            var testDataTable = new DataTable();
            testDataTable.Columns.Add("Description");
            testDataTable.Columns.Add("Manufacturer");
            testDataTable.Columns.Add("Model");
            testDataTable.Columns.Add("HorsePower");
            testDataTable.Columns.Add("EngineSize");
            testDataTable.Columns.Add("PetrolType");
            testDataTable.Columns.Add("FuelTankSize");
            testDataTable.Columns.Add("Weight");
            testDataTable.Columns.Add("TopSpeed");
            testDataTable.Columns.Add("Acceleration");
            testDataTable.Columns.Add("AvgFuelConsumption");
            testDataTable.Columns.Add("ProductionYear");

            testDataTable.Rows.Add("TestCar1", "Mazda", "RX8", "251", "1308", "gasoline", "60", "900", "270", (5.4M).ToString(), (9.2M).ToString(), "2013");
            testDataTable.Rows.Add("TestCar2", "Honda", "ASX", "320", "2800", "gasoline", "60", "900", "270", (5.4M).ToString(), (15.4M).ToString(), "2007");

            var mapper = new DataTableMapper(_logger);

            var carDescriptions = mapper.ConvertToDbEnumerable<CarDescription>(testDataTable, new Attribute[] { new PresentableItem() }).ToArray();

            _logger.DidNotReceive().Error(Arg.Any<string>());

            Assert.Equal(2, carDescriptions.Count());
            Assert.Equal("TestCar1", carDescriptions[0].Description);
            Assert.Equal("Mazda", carDescriptions[0].Manufacturer);
            Assert.Equal(9.2M, carDescriptions[0].AvgFuelConsumption);

            Assert.Equal("TestCar2", carDescriptions[1].Description);
            Assert.Equal("Honda", carDescriptions[1].Manufacturer);
            Assert.Equal(15.4M, carDescriptions[1].AvgFuelConsumption);
        }
    }
}
