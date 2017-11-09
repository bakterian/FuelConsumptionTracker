using FCT.Bootstrapper;
using System;
using System.Windows;

namespace FCT.App.Window
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            #if (DEBUG)
            RunInDebugMode();
            #else
            RunInReleaseMode();
            #endif
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private static void RunInDebugMode()
        {
            var rootView = RunBootsrapper();
            ShowMainWindow(rootView);
        }

        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            try
            {
                var rootView = RunBootsrapper();
                ShowMainWindow(rootView);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            MessageBox.Show("Captured unhalted exception");
            Environment.Exit(1);
        }

        private static Shell RunBootsrapper()
        {
            var bootstrapper = new Bootstrappy();
            var rootView = bootstrapper.Run();
            return rootView;
        }

        private static void ShowMainWindow(Shell rootView)
        { 
            Application.Current.MainWindow = rootView;
            Application.Current.MainWindow.Show();
        }
    }
}
