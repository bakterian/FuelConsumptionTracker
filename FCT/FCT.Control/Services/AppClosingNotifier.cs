using System.ComponentModel;
using FCT.Infrastructure.Interfaces;
using System.Collections.Generic;
using FCT.Infrastructure.Enums;
using System.Linq;
using System;

namespace FCT.Control.Services
{
    public class AppClosingNotifier : IAppClosingNotifier
    {
        private List<Tuple<INotifyAppClosing, NotificationPriority>> _listeners;

        public AppClosingNotifier(System.Windows.Window mainAppWindow)
        {
            _listeners = new List<Tuple<INotifyAppClosing, NotificationPriority>> ();
            mainAppWindow.Closing -= OnMainWindowClosing;
            mainAppWindow.Closing += OnMainWindowClosing;
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            foreach (var listener in _listeners.OrderByDescending(_ => _.Item2))
            {
                listener.Item1.OnApplicationClose();
            }
        }

        public void RegisterForNotification(INotifyAppClosing closeListener,NotificationPriority priority)
        {
            if(!_listeners.Select(_ => _.Item1).Contains(closeListener))
            {
                _listeners.Add(new Tuple<INotifyAppClosing, NotificationPriority>(closeListener,priority));
            }
        }
    }
}
