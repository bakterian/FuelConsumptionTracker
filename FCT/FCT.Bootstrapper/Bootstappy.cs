using FCT.Control.KernelModules;
using FCT.DataAccess.KernelModules;
using FCT.Infrastructure.Services;
using Ninject;

namespace FCT.Bootstrapper
{
    public class Bootstrappy
    {
        private IKernel _kernel;

        public Bootstrappy()
        {
            _kernel = new StandardKernel();
        }

        public void Run()
        {//To be triggered from FCT.App.Window
            LoadCommonModule();
            LoadWindowModule();
        }

        public void RunInConsole()
        {//To be triggered from FCT.App.Console
            LoadCommonModule();
            LoadConsoleModule();
            StartConsoleSession();
        }

        private void LoadConsoleModule()
        {
            var consoleModule = new ConsoleModule();
            _kernel.Load(consoleModule);
        }

        private void LoadWindowModule()
        {
            var windowModule = new WindowModule();
            _kernel.Load(windowModule);
        }

        private void LoadCommonModule()
        {
            var dataAccessModule = new DataAccessModule();
            _kernel.Load(dataAccessModule);
        }

        private void StartConsoleSession()
        {
            var cr = _kernel.Get<IConsoleRunner>();
            cr.RunConsoleSession();
        }
    }
}
