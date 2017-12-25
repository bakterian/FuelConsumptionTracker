using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface ISpreadsheetWriter
    {
        Task<bool> WriteToExcelFileAsync(string filePath, IEnumerable<Tuple<string, DataTable>> worksheetPayloads);

        bool WriteToExcelFile(string filePath, IEnumerable<Tuple<string, DataTable>> worksheetPayloads);
    }
}
