using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using System;

namespace FCT.Control.ViewModels
{
    public class FileMenuViewModel : RegionBaseViewModel, IFileMenuViewModel
    {
        private readonly IThemeSwitcher _themeSwitcher;
        public FileMenuViewModel(IThemeSwitcher themeSwitcher)
        {
            _themeSwitcher = themeSwitcher;
        }   
        public override void Initialize()
        {
            
        }

        public void OnFileOpen()
        {

        }

        public void OnFileSave()
        {

        }

        public void OnThemeChange(string theme)
        {
            if (Enum.TryParse(theme, out AppTheme res))
            {
                if(_themeSwitcher.GetActiveTheme() != res)
                {
                    var switchSuccessfull = _themeSwitcher.SetTheme(res);
                    if(!switchSuccessfull)
                    {
                        Console.WriteLine("theme switching was not successfull");
                        //TODO: froward to logger; Show toast message that something went wrong
                    }
                }
            }
            else
            {
                //TODO: froward to logger; Show toast message that something went wrong
                Console.WriteLine($"Invalid argument passed to OnThemeChanged: {theme}.");
            }
        }

        public void OnDbInfo()
        {

        }

        public void OnAbout()
        {

        }

    }
}
