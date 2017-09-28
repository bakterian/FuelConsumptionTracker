using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Services;
using System;

namespace FCT.Control.Services
{
    public class ConsoleRunner : IConsoleRunner
    {//TODO: Test this!!!
        private IDbDataReader _dbDataReader;

        public ConsoleRunner(IDbDataReader dbDataReader)
        {
            _dbDataReader = dbDataReader;
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
            switch(userOption)
            {
                case ConsoleUserOption.GetAllCarDescriptions:
                    var carDescriptions = _dbDataReader.GetCarDescriptions();
                    carDescriptions.ForEach(_ => Console.WriteLine(_.Summary+"\n"));
                    break;
                case ConsoleUserOption.GetAllFuelConsumptionEntries:
                    var fuelConsEntries = _dbDataReader.GetFuelConEntries();
                    fuelConsEntries.ForEach(_ => Console.WriteLine(_.Summary+ "\n"));
                    break;
                case ConsoleUserOption.GetAllFuelConEntriesByCarId:
                    Console.WriteLine("Specify the car id:");
                    var carId = GetCarIdUserInput();
                    if(carId.HasValue)
                    {
                        var fuelConsByCarId = _dbDataReader.GetFuelConEntries(carId.Value);
                        fuelConsByCarId.ForEach(_ => Console.WriteLine(_.Summary+ "\n"));
                    }
                    else
                    {
                        Console.WriteLine("Inavlid car id user input, aborting...\n");
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
            "Please press the chosen number and hit enter:\n"+
            "1 - Get all car descriptions\n"+
            "2 - Get all fuel consumption entries\n"+
            "3 - Get all fuel consumption entries by car id\n" +
            "4 - Insert car description\n"+
            "5 - Insert fuel consumption\n"+
            "6 - Update fuel consumption\n"+
            "7 - Close program"
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
            var readKey = Console.ReadLine();
            int userSelection;
            var convResult = int.TryParse(readKey, out userSelection);

            int? sel = userSelection;
            if (convResult == false) sel = null;
            return userSelection;
        }
    }
}
