﻿using Ninject.Modules;
using FCT.Infrastructure.Services;
using FCT.Control.Services;

namespace FCT.Control.KernelModules
{
    public class ConsoleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConsoleRunner>().To<ConsoleRunner>().InSingletonScope();
        }
    }
}
