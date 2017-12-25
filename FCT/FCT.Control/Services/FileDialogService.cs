using FCT.Infrastructure.Interfaces;
using Microsoft.Win32;

namespace FCT.Control.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string GetSaveFilePath()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "Excel File|*.xlsx";
            saveFileDialog.Title = "Save an xlsx excel file.";
            var receivedPath = saveFileDialog.ShowDialog();

            if(!receivedPath.Value) saveFileDialog.FileName = "Operation Aborted";

            return saveFileDialog.FileName;
        }

        public string GetOpenFilePath()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Excel File|*.xlsx";
            openFileDialog.Title = "Open an xlsx excel file.";
            var receivedPath = openFileDialog.ShowDialog();

            if (!receivedPath.Value) openFileDialog.FileName = "Operation Aborted";

            return openFileDialog.FileName;
        }
    }
}
