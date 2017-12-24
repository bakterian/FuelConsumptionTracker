using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface ISpreadsheetGoverner
    {
        Task<bool> SaveTableDataToExcelFileAsync(string filePath);

        bool SaveTableDataToExcelFile(string filePath);

        Task<bool> LoadTableDatafromExcelFileAsync(string filePath);

        bool LoadTableDatafromExcelFile(string filePath);
    }
}
