namespace FCT.Infrastructure.Interfaces
{
    public interface IAppClosingNotifier
    {
        void RegisterForNotification(INotifyAppClosing closeListener);
    }
}
