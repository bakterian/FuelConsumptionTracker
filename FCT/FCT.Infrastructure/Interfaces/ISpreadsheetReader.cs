using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface ISpreadsheetReader
    {
        Task<IList<DataTable>> ReadFromExcelFileAsync(string filePath);

        IList<DataTable> ReadFromExcelFile(string filePath);
    }
}
