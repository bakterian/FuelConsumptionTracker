using Ninject.Modules;
using FCT.DataAccess.Services;
using FCT.Infrastructure.Interfaces;

namespace FCT.DataAccess.KernelModules
{
    public class DataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbReader>().To<DbReader>().InSingletonScope();
            Bind<IDbWriter>().To<DbWriter>().InSingletonScope();
            Bind<IDbInfoProvider>().To<DbInfoProvider>().InSingletonScope();
        }
    }
}
