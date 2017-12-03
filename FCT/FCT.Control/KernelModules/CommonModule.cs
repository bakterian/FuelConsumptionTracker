using FCT.Control.Services;
using FCT.Infrastructure.Interfaces;
using Ninject.Modules;

namespace FCT.Control.KernelModules
{
    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger>().InSingletonScope();
        }
    }
}
