using FCT.Infrastructure.Enums;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDbActionsNotifier
    {
        void RegisterForNotification(INotifyDbActions dbActionListener, NotificationPriority priority);

        void FireWriteNotification();

        void FireReadNotification();
    }
}
