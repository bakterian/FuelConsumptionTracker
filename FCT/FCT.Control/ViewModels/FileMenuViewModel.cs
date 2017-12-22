using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using System;

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
        {

        }

        public void OnFileSave()
        {   //WA for Caliburn as it does not support async methods
            //TODO: ask user to save data in the DB if there is dirty data
            SaveFile();
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

        private async void SaveFile()
        {
            var saveFilePath = _fileDialogService.GetSaveFilePath();
            if (string.IsNullOrEmpty(saveFilePath))
            {
                PopulateErrorMsg("File Save", "The specified path is invalid.");
            }
            else
            {
                var saveWasSuccessfull = await _spreadsheetGoverner.SaveDbDataToExcelFileAsync(saveFilePath);
                //consider displaying a spinner if these operation take to long
                // the spinner would stop at this point

                if (saveWasSuccessfull)
                {
                    PopulateInfoMsg("File Save", "Save file operation was successfull.");
                }
                else
                {
                    PopulateErrorMsg("File Save", "Save file operation was  not successfull.");
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

    }
}
