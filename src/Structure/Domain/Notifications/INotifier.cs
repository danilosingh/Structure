namespace Structure.Domain.Notifications
{
    public interface INotifier
    {
        void Notify(string message);
        void Notify(string message, string member);
        void Notify(INotification notification);
    }
}
