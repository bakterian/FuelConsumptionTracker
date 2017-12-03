using FCT.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace FCT.Control.Services
{
    public class DbActionsNotifier : IDbActionsNotifier
    {
        private readonly IList<INotifyDbActions> _listeners;

        public DbActionsNotifier()
        {
            _listeners = new List<INotifyDbActions>();
        }

        public void RegisterForNotification(INotifyDbActions dbActionListener)
        {
            _listeners.Add(dbActionListener);
        }

        public void FireReadNotification()
        {
            foreach (var listener in _listeners)
            {
                listener.OnDbRead();
            }
        }

        public void FireWriteNotification()
        {
            foreach (var listener in _listeners)
            {
                listener.OnDbWrite();
            }
        }
    }
}
