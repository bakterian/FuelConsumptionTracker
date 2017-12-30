using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCT.Control.Services
{
    public class DbActionsNotifier : IDbActionsNotifier
    {
        private readonly IList<Tuple<INotifyDbActions,NotificationPriority>> _listeners;

        public DbActionsNotifier()
        {
            _listeners = new List<Tuple<INotifyDbActions, NotificationPriority>>();
        }

        public void RegisterForNotification(INotifyDbActions dbActionListener, NotificationPriority priority)
        {
            if (!_listeners.Select(_ => _.Item1).Contains(dbActionListener))
            {
                _listeners.Add(new Tuple<INotifyDbActions, NotificationPriority>(dbActionListener, priority));
            }
        }

        public void FireReadNotification()
        {
            foreach (var listener in _listeners.OrderByDescending(_ => _.Item2))
            {
                listener.Item1.OnDbRead();
            }
        }

        public void FireWriteNotification()
        {
            foreach (var listener in _listeners.OrderByDescending(_ => _.Item2))
            {
                listener.Item1.OnDbWrite();
            }
        }
    }
}
