using System.ComponentModel;
using FCT.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace FCT.Control.Services
{
    public class AppClosingNotifier : IAppClosingNotifier
    {
        private List<INotifyAppClosing> _listeners;

        public AppClosingNotifier(System.Windows.Window mainAppWindow)
        {
            _listeners = new List<INotifyAppClosing>();
            mainAppWindow.Closing -= OnMainWindowClosing;
            mainAppWindow.Closing += OnMainWindowClosing;
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            foreach (var listener in _listeners)
            {
                listener.OnApplicationClose();
            }
        }

        public void RegisterForNotification(INotifyAppClosing closeListener)
        {
            if(!_listeners.Contains(closeListener))
            {
                _listeners.Add(closeListener);
            }
        }
    }
}
