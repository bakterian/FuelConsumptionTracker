using System;
using Ninject.Modules;
using FCT.Infrastructure.Interfaces;
using FCT.Control.ViewModels;
using FCT.Control.Services;

namespace FCT.Control.KernelModules
{
    public class WindowModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICarSelectionViewModel>().To<CarSelectionViewModel>().InSingletonScope();
            Bind<IFileMenuViewModel>().To<FileMenuViewModel>().InSingletonScope();
        }
    }
}
