
namespace FCT.Infrastructure.Interfaces
{
    public interface IDbActionsNotifier
    {
        void RegisterForNotification(INotifyDbActions dbActionListener);

        void FireWriteNotification();

        void FireReadNotification();
    }
}
