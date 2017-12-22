using System;
using System.Threading.Tasks;
using FCT.Infrastructure.Interfaces;
using System.Data;
using FCT.Infrastructure.Attributes;
using System.Linq;

namespace FCT.Control.Services
{
    public class SpreadsheetGoverner : ISpreadsheetGoverner
    {
        private readonly IDbReader _dbReader;
        private readonly IDataTableMapper _dataTableMapper;
        private readonly ISpreadsheetReader _spreadsheetReader;
        private readonly ISpreadsheetWriter _spreadsheetWriter;
        private readonly ILogger _logger;

        public SpreadsheetGoverner(
            IDbReader dbReader,
            IDataTableMapper dataTableMapper,
            ISpreadsheetReader spreadsheetReader,
            ISpreadsheetWriter spreadsheetWriter,
            ILogger logger
            )
        {
            _dbReader = dbReader;
            _dataTableMapper = dataTableMapper;
            _spreadsheetReader = spreadsheetReader;
            _spreadsheetWriter = spreadsheetWriter;
            _logger = logger;
        }

        public Task<bool> SaveDbDataToExcelFileAsync(string filePath)
        {
            return Task.FromResult<bool>(SaveDbDataToExcelFile(filePath));
        }

        public bool SaveDbDataToExcelFile(string filePath)
        {
            var saveSuccessfull = false;
            var fuelConsEntries = _dbReader.GetFuelConEntries();
            var carDescriptions = _dbReader.GetCarDescriptions();

            var fuelConsEntTableTask = _dataTableMapper.ConvertToDataTableAsync(fuelConsEntries, new[] { new PresentableItem() });
            var carDesTableTask = _dataTableMapper.ConvertToDataTableAsync(carDescriptions, new[] { new PresentableItem() });

            object tasks;
            try
            {
                tasks = Task.WhenAll(new Task<DataTable>[] { fuelConsEntTableTask, carDesTableTask });
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Exceprion was thrown during db data to data table conversions.");
                return saveSuccessfull;
            }


            var worksheetHeadings = new[] { "FuelConsEntries", "CarDescriptions" };
            foreach (var dataTable in ((Task<DataTable[]>)tasks).Result)
            {

            }
            var dataTablesWithHeading = (((Task<DataTable[]>)tasks).Result).
                Select((_,i) => new Tuple<string, DataTable>(worksheetHeadings[i++], _));

            saveSuccessfull = _spreadsheetWriter.WriteToExcelFile(filePath, dataTablesWithHeading);

            return saveSuccessfull;
        }
    }
}
