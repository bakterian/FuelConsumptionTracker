using System;
using System.Threading.Tasks;
using FCT.Infrastructure.Interfaces;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace FCT.Control.Services
{
    public class SpreadsheetGoverner : ISpreadsheetGoverner
    {
        private readonly IDbReader _dbReader;
        private readonly IDataTableMapper _dataTableMapper;
        private readonly ISpreadsheetReader _spreadsheetReader;
        private readonly ISpreadsheetWriter _spreadsheetWriter;
        private readonly IDbTabVmStore _dbTabVmStore;
        private readonly ILogger _logger;

        public SpreadsheetGoverner(
            IDbReader dbReader,
            IDataTableMapper dataTableMapper,
            ISpreadsheetReader spreadsheetReader,
            ISpreadsheetWriter spreadsheetWriter,
            IDbTabVmStore dbTabVmStore,
            ILogger logger
            )
        {
            _dbReader = dbReader;
            _dataTableMapper = dataTableMapper;
            _spreadsheetReader = spreadsheetReader;
            _spreadsheetWriter = spreadsheetWriter;
            _dbTabVmStore = dbTabVmStore;
            _logger = logger;
        }

        public Task<bool> SaveTableDataToExcelFileAsync(string filePath)
        {
            return Task.FromResult<bool>(SaveTableDataToExcelFile(filePath));
        }

        public bool SaveTableDataToExcelFile(string filePath)
        {
            var saveSuccessfull = false;
            var worksheetHeadings = new List<string>();
            var dataTableCreationTasks = new List<Task<DataTable>>();

            foreach (var dbTabVms in _dbTabVmStore.GetAll())
            {
                worksheetHeadings.Add(dbTabVms.HeaderName);
                dataTableCreationTasks.Add(dbTabVms.GetDataTableAsync());
            }

            object tasks;
            try
            {
                tasks = Task.WhenAll(dataTableCreationTasks);
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Exception was thrown during db data to data table conversions.");
                return saveSuccessfull;
            }

            var dataTablesWithHeading = (((Task<DataTable[]>)tasks).Result).
                Select((_,i) => new Tuple<string, DataTable>(worksheetHeadings[i++], _));

            saveSuccessfull = _spreadsheetWriter.WriteToExcelFile(filePath, dataTablesWithHeading);

            return saveSuccessfull;
        }

        public Task<bool> LoadTableDatafromExcelFileAsync(string filePath)
        {
            return Task.FromResult<bool>(LoadTableDatafromExcelFile(filePath));
        }

        public bool LoadTableDatafromExcelFile(string filePath)
        {
            var loadSuccessfull = false;
            var dataTables = _spreadsheetReader.ReadFromExcelFile(filePath);
            var presentationUpdateTasks = new List<Task>();

            foreach (var dbTabVms in _dbTabVmStore.GetAll())
            {
                presentationUpdateTasks.Add(dbTabVms.UpdateDataAsync(dataTables));
            }

            object tasks;
            try
            {
                tasks = Task.WhenAll(presentationUpdateTasks);
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Exception was thrown during data table to presentend data conversions.");
                return loadSuccessfull;
            }

            loadSuccessfull = true;

            return loadSuccessfull;
        }
    }
}
