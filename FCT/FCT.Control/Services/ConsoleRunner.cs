using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Models;
using FCT.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCT.Control.Services
{
    public class ConsoleRunner : IConsoleRunner
    {//TODO: Test this!!!
        private IDbReader _dbReader;
        private IDbWriter _dbWriter;
        public ConsoleRunner(
            IDbReader dbReader,
            IDbWriter dbWriter
            )
        {
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }
        public void RunConsoleSession()
        {
            DisplayInitialInfo();
            while (true)
            {
                Console.WriteLine("Select option...\n");
                var userSel = GetUserSelection();
                HandleResponse(userSel);
            }
        }

        private void HandleResponse(ConsoleUserOption userOption)
        {
            switch (userOption)
            {
                case ConsoleUserOption.GetAllCarDescriptions:
                    var carDescriptions = _dbReader.GetCarDescriptions();
                    PrintModelSummaries(carDescriptions);
                    break;
                case ConsoleUserOption.GetAllFuelConsumptionEntries:
                    var fuelConsEntries = _dbReader.GetFuelConEntries();
                    PrintModelSummaries(fuelConsEntries);
                    break;
                case ConsoleUserOption.GetAllFuelConEntriesByCarId:
                    var carId = GetCarIdUserInput();
                    if (carId.HasValue)
                    {
                        var fuelConsByCarId = _dbReader.GetFuelConEntries(carId.Value);
                        PrintModelSummaries(fuelConsByCarId);
                    }
                    else
                    {
                        Console.WriteLine("Inavlid car id user input, aborting...\n");
                    }
                    break;
                case ConsoleUserOption.InsertCarDescription:
                    var carDescription = GetCarDescriptionText();
                    if (IsValidText(carDescription))
                    {
                        InsertCarDescription(carDescription);
                    }
                    else
                    {
                        Console.WriteLine("Invalid car description was entered, aborting...\n");
                    }
                    break;
                case ConsoleUserOption.UpdateCarDescription:
                    var id = GetCarIdUserInput();
                    if (id.HasValue)
                    {
                        var carDesToUpdate = _dbReader.GetCarDescriptions().Where(_ => _.Id == id).FirstOrDefault();
                        if (carDesToUpdate == null)
                        {
                            Console.WriteLine($"There is no car with given Id={id}, aborting...\n");
                        }
                        else
                        {
                            var newCarDesText = GetCarDescriptionText();
                            if (IsValidText(newCarDesText))
                            {
                                UpdateCarDescription(newCarDesText, carDesToUpdate);
                            }
                            else
                            {
                                Console.WriteLine("Invalid car description was entered, aborting...\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid car id was entered, aborting...\n");
                    }
                    break;
                case ConsoleUserOption.InsertFuelConsumption:
                    var userCarId = GetCarIdUserInput();
                    if (userCarId.HasValue)
                    {
                        var carDesToUpdate = _dbReader.GetCarDescriptions().Where(_ => _.Id == userCarId).FirstOrDefault();
                        if (carDesToUpdate == null)
                        {
                            Console.WriteLine($"There is no car with given Id={userCarId}, aborting...\n");
                        }
                        else
                        {
                            var petrolStationDesc = GetPetrolStationDescriptionText();
                            if (IsValidText(petrolStationDesc))
                            {
                                InsertFuelConsumption(petrolStationDesc, userCarId.Value);
                            }
                            else
                            {
                                Console.WriteLine("Invalid petrol station description was entered, aborting...\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid car id was entered, aborting...\n");
                    }
                    break;
                case ConsoleUserOption.UpdateFuelConsumption:
                    var fuelEntryId = GetFuelEntryIdUserInput();
                    if (fuelEntryId.HasValue)
                    {
                        var fuelConEntryToUpdate = _dbReader.GetFuelConEntries().Where(_ => _.Id == fuelEntryId).FirstOrDefault();
                        if (fuelConEntryToUpdate == null)
                        {
                            Console.WriteLine($"There is no fuel consumption entry with given Id={fuelEntryId}, aborting...\n");
                        }
                        else
                        {
                            var petrolStationDesc = GetPetrolStationDescriptionText();
                            if (IsValidText(petrolStationDesc))
                            {
                                UpdateFuelConEntry(petrolStationDesc, fuelConEntryToUpdate);
                            }
                            else
                            {
                                Console.WriteLine("Invalid petrol station description was entered, aborting...\n");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid fuel consumption entry id was entered, aborting...\n");
                    }
                    break;

                case ConsoleUserOption.Invalid:
                    Console.WriteLine("Inavlid option selection.\n");
                    DisplayInitialInfo();
                    break;
                case ConsoleUserOption.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Option not yet supported.\n");
                    break;
            }
        }

        private void DisplayInitialInfo()
        {
            Console.WriteLine
            (
            "Please press the chosen number and hit enter:\n" +
            "1 - Get all car descriptions\n" +
            "2 - Get all fuel consumption entries\n" +
            "3 - Get all fuel consumption entries by car id\n" +
            "4 - Insert car description\n" +
            "5 - Update car description\n" +
            "6 - Insert fuel consumption\n" +
            "7 - Update fuel consumption\n" +
            "8 - Close program"
            );
        }

        private ConsoleUserOption GetUserSelection()
        {
            var readKey = Console.ReadLine();
            ConsoleUserOption userSelection;
            var convResult = Enum.TryParse(readKey, out userSelection);

            if (convResult == false) userSelection = ConsoleUserOption.Invalid;

            return userSelection;
        }

        private int? GetCarIdUserInput()
        {
            return GetUserIntValue("Specify the car id:");
        }

        private int? GetFuelEntryIdUserInput()
        {
            return GetUserIntValue("Specify the fuel entry id:");
        }

        private int? GetUserIntValue(string initalText)
        {
            Console.WriteLine(initalText);
            var readKey = Console.ReadLine();
            int userSelection;
            var convResult = int.TryParse(readKey, out userSelection);
            int? sel = userSelection;
            if (convResult == false) sel = null;
            return userSelection;
        }

        private string GetCarDescriptionText()
        {
            Console.WriteLine("Please enter Description for your Mazda Rx8.");
            return Console.ReadLine();
        }

        private string GetPetrolStationDescriptionText()
        {
            Console.WriteLine("Please enter the pertol station description.");
            return Console.ReadLine();
        }

        private bool IsValidText(string carDesc)
        {
            return carDesc != null && carDesc != string.Empty;
        }

        private void InsertCarDescription(string carDescription)
        {
            var carDesc = new CarDescription()
            {
                Description = carDescription,
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

            var carDescriptions = new CarDescription[] { carDesc };
            var affectedRows = _dbWriter.InsertCarDescriptions(carDescriptions);

            PrintConfirmationMassage(affectedRows);
        }

        private void InsertFuelConsumption(string petrolStationDesc, int carId)
        {
            var fuelConEntry = new FuelConEntry()
            {
                CarId = carId,
                PetrolStationDesc = petrolStationDesc,
                PetrolType = "gasoline",
                FuelingDate = GetCurrentDateTime(),
                LiterAmount = 100.0M,
                PricePerLiter = 4.5M,
                FullPrice = 450.0M,
                DistanceMade = 3000.0M,
                Terrain = "highways"
            };
            fuelConEntry.FuelConsumption = fuelConEntry.DistanceMade / fuelConEntry.LiterAmount;

            var fuelConEntrys = new FuelConEntry[] { fuelConEntry };
            var affectedRows = _dbWriter.InsertFuelConsumptions(fuelConEntrys);

            PrintConfirmationMassage(affectedRows);
        }

        private void UpdateCarDescription(string newCarDescription, CarDescription carDescToUpdate)
        {
            carDescToUpdate.Description = newCarDescription;
            var carDescriptions = new CarDescription[] { carDescToUpdate };
            var affectedRows = _dbWriter.UpdateCarDescriptions(carDescriptions);
            PrintConfirmationMassage(affectedRows);
        }

        private void UpdateFuelConEntry(string newPetrolStationDesc, FuelConEntry fuelConsEntryToUpdate)
        {
            fuelConsEntryToUpdate.PetrolStationDesc = newPetrolStationDesc;
            var fuelConEntrys = new FuelConEntry[] { fuelConsEntryToUpdate };
            var affectedRows = _dbWriter.UpdateFuelConsumptions(fuelConEntrys);
            PrintConfirmationMassage(affectedRows);
        }

        private void PrintConfirmationMassage(int affectedRows)
        {
            var confMsg = affectedRows == -1 ? "operation was unsuccessful." : "operation was successful.";
            Console.WriteLine(confMsg);
        }

        private void PrintModelSummaries(IEnumerable<BaseDbModel> dbModels)
        {
            foreach (var dbModel in dbModels)
            {
                Console.WriteLine(dbModel.Summary + "\n");
            }
        }

        private DateTime GetCurrentDateTime()
        {
            return new DateTime(DateTime.Now.Ticks);
        }
    }
}
