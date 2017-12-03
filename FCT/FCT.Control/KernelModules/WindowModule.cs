using System;
using Ninject.Modules;
using FCT.Infrastructure.Interfaces;
using FCT.Control.ViewModels;
using FCT.Control.Services;
using System.Windows;
using System.Collections.Generic;
using FCT.Infrastructure.Models;
using FCT.Infrastructure.Enums;

namespace FCT.Control.KernelModules
{
    public class WindowModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICarSelectionViewModel>().To<CarSelectionViewModel>().InSingletonScope();
            Bind<IFileMenuViewModel>().To<FileMenuViewModel>().InSingletonScope();
            Bind<ICarDataViewModel>().To<CarDataViewModel>().InSingletonScope();
            Bind<IFuelConsumptionViewModel>().To<FuelConsumptionViewModel>().InSingletonScope();
            Bind<IStatisticsViewModel>().To<StatisticsViewModel>().InSingletonScope();
            Bind<IMainTabViewModel>().To<MainTabViewModel>().InSingletonScope();
            Bind<IThemeSwitcher>().ToConstant(GetThemeSwitcher()).InSingletonScope();
            Bind<IDialogService>().To<DialogService>().InSingletonScope();
            Bind<IDbActionsNotifier>().To<DbActionsNotifier>().InSingletonScope();
        }

        private ThemeSwitcher GetThemeSwitcher()
        {
            var resourceDictionaries = Application.Current.Resources.MergedDictionaries;
            var themeMapping = new List<ThemeMap>()
            {
                new ThemeMap()
                {
                    Theme = AppTheme.AeroDark,
                    GeneralSourceUri = new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml", UriKind.Relative),
                    ColorSourceUri = new Uri("/FCT.Control;component/Resources/Themes/DarkTheme.xaml", System.UriKind.Relative)
                },
                new ThemeMap()
                {
                    Theme = AppTheme.AeroWhite,
                    GeneralSourceUri = new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml", UriKind.Relative),
                    ColorSourceUri = new Uri("/FCT.Control;component/Resources/Themes/WhiteTheme.xaml", System.UriKind.Relative)
                },
                new ThemeMap()
                {
                    Theme = AppTheme.RoyalDark,
                    GeneralSourceUri = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", System.UriKind.Relative),
                    ColorSourceUri = new Uri("/FCT.Control;component/Resources/Themes/DarkTheme.xaml", System.UriKind.Relative)
                },
                new ThemeMap()
                {
                    Theme = AppTheme.RoyalWhite,
                    GeneralSourceUri = new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml", System.UriKind.Relative),
                    ColorSourceUri = new Uri("/FCT.Control;component/Resources/Themes/WhiteTheme.xaml", System.UriKind.Relative)
                }
            }; 

            var themeSwitcher = new ThemeSwitcher(themeMapping,resourceDictionaries);

            return themeSwitcher;
        }
    }
}
