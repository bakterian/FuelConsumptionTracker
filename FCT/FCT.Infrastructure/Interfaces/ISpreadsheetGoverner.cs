
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface ISpreadsheetGoverner
    {
        Task<bool> SaveDbDataToExcelFileAsync(string filePath);

        bool SaveDbDataToExcelFile(string filePath);
    }
}
