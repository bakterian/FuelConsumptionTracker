using FCT.Control.KernelModules;
using FCT.Control.RegionModules;
using FCT.DataAccess.KernelModules;
using FCT.Infrastructure.Interfaces;
using Ninject;
//TODO: remove un-need presentation interfaces from this project
namespace FCT.Bootstrapper
{
    public class Bootstrappy
    {
        private IKernel _kernel;

        public Bootstrappy()
        {
            _kernel = new StandardKernel();
        }

        public Shell Run()
        {//To be triggered from FCT.App.Window
            LoadCommonModule();
            LoadWindowModule();
            var root = GetInitializedRootView();
            return root;
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

        private Shell GetInitializedRootView()
        {
            var rootView = new Shell();
            rootView = LoadRegions(rootView);
            return rootView;
        }

        private Shell LoadRegions(Shell rootView)
        {
            var regionConfigurator = new RegionConfigurator();
            regionConfigurator.LoadRegions(new WindowRegionCollection());

            var bindedRootView  = regionConfigurator.Configure(rootView, _kernel);
            return bindedRootView;
        }
    }
}
