using System;
using FCT.Infrastructure.Interfaces;
using System.Windows;
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
            saveFileDialog.ShowDialog();

            return saveFileDialog.FileName;
        }
    }
}
