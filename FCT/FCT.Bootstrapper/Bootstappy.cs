using System;
using System.ComponentModel;
using FCT.Control.KernelModules;
using FCT.Control.RegionModules;
using FCT.DataAccess.KernelModules;
using FCT.Infrastructure.Interfaces;
using Ninject;
using FCT.Control.Services;
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
            _kernel.Get<ILogger>().Information("Staring window app");
            var root = GetInitializedRootView();
            return root;
        }

        public void RunInConsole()
        {//To be triggered from FCT.App.Console
            LoadCommonModule();
            LoadConsoleModule();
            StartConsoleSession();
            _kernel.Get<ILogger>().Information("Staring console app");
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
            var commonModule = new CommonModule();
            var dataAccessModule = new DataAccessModule();
            _kernel.Load(dataAccessModule, commonModule);
        }

        private void StartConsoleSession()
        {
            var cr = _kernel.Get<IConsoleRunner>();
            cr.RunConsoleSession();
        }

        private Shell GetInitializedRootView()
        {
            var rootView = new Shell();
            _kernel.Bind<IAppClosingNotifier>().ToConstant(new AppClosingNotifier(rootView));
            rootView = LoadRegions(rootView);
            return rootView;
        }

        private void OnAppClosing(object sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
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
