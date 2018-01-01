using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using System;
using System.IO;

namespace FCT.Control.ViewModels
{
    public class FileMenuViewModel : RegionBaseViewModel, IFileMenuViewModel
    {
        private readonly IThemeSwitcher _themeSwitcher;
        private readonly ILogger _logger;
        private readonly IFileDialogService _fileDialogService;
        private readonly IDialogService _dialogService;
        private readonly ISpreadsheetGoverner _spreadsheetGoverner;

        public FileMenuViewModel(
            IThemeSwitcher themeSwitcher,
            ILogger logger,
            IFileDialogService fileDialogService,
            IDialogService dialogService,
            ISpreadsheetGoverner spreadsheetGoverner
            )
        {
            _themeSwitcher = themeSwitcher;
            _logger = logger;
            _fileDialogService = fileDialogService;
            _dialogService = dialogService;
            _spreadsheetGoverner = spreadsheetGoverner;
        }   

        public override void Initialize()
        {
            
        }

        public void OnFileOpen()
        {   //WA for Caliburn as it does not support async methods
            LoadFile();
        }

        public void OnFileSave()
        {   //WA for Caliburn as it does not support async methods
            SaveFile();
        }

        public void OnExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void OnThemeChange(string theme)
        {
            string errorMsg = null;
            if (Enum.TryParse(theme, out AppTheme res))
            {
                if(_themeSwitcher.GetActiveTheme() != res)
                {
                    var switchSuccessfull = _themeSwitcher.SetTheme(res);
                    if(!switchSuccessfull)
                    {
                        errorMsg = "Theme switching was not successfull.";
                    }
                }
            }
            else
            {
                errorMsg = $"Invalid argument passed to OnThemeChanged: {theme}.";
            }

            if (errorMsg != null) PopulateErrorMsg("theme switching", errorMsg);
        }

        public void OnDbInfo()
        {

        }

        public void OnAbout()
        {

        }

        private async void LoadFile()
        {
            var openFilePath = _fileDialogService.GetOpenFilePath();

            if (openFilePath.Equals("Operation Aborted"))
            {
                _logger.Information("[FileMenuViewModel] Load operation was aborted.");
            }

            else if (string.IsNullOrEmpty(openFilePath) || !(new FileInfo(openFilePath).Exists))
            {
                PopulateErrorMsg("File Open", "The specified path is invalid.");
            }
            else if (IsFileLocked(new FileInfo(openFilePath)))
            {
                PopulateErrorMsg("File Open", "The specified is locked.\nPlease make sure that the file is not opened in other programs.");
            }
            else
            {
                var loadWasSuccessfull = await _spreadsheetGoverner.LoadTableDatafromExcelFileAsync(openFilePath);

                if (loadWasSuccessfull)
                {
                    PopulateInfoMsg("File Load", "Load file operation was successfull.");
                }
                else
                {
                    PopulateErrorMsg("File Load", "Load file operation was not successfull.\nCheck error log for details.");
                }
            }
        }

        private async void SaveFile()
        {
            var saveFilePath = _fileDialogService.GetSaveFilePath();

            if (saveFilePath.Equals("Operation Aborted"))
            {
                _logger.Information("[FileMenuViewModel] Save operation was aborted.");
            }

            else if (string.IsNullOrEmpty(saveFilePath))
            {
                PopulateErrorMsg("File Save", "The specified path is invalid.");
            }
            else if((new FileInfo(saveFilePath)).Exists && IsFileLocked(new FileInfo(saveFilePath)))
            {
                PopulateErrorMsg("File Save", "The specified is locked.\nPlease make sure that the file is not opened in other programs.");
            }
            else
            {
                var saveWasSuccessfull = await _spreadsheetGoverner.SaveTableDataToExcelFileAsync(saveFilePath);
                //consider displaying a spinner if these operation take to long
                // the spinner would stop at this point

                if (saveWasSuccessfull)
                {
                    PopulateInfoMsg("File Save", "Save file operation was successfull.");
                }
                else
                {
                    PopulateErrorMsg("File Save", "Save file operation was  not successfull.\nCheck error log for details.");
                }
            }
        }

        private void PopulateErrorMsg(string caption, string errorMsg)
        {
            _logger.Error(errorMsg);
            _dialogService.ShowErrorMsg(caption, errorMsg);
            //TODO: switch to toast message in stead of the old pop-up dialog
        }

        private void PopulateInfoMsg(string caption, string infoMsg)
        {
            _logger.Information(infoMsg);
            _dialogService.ShowInfoMsg(caption, infoMsg);
            //TODO: switch to toast message in stead of the old pop-up dialog
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            var fileIsLocked = false;
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                fileIsLocked = true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return fileIsLocked;
        }
    }
}
