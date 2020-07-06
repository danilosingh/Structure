using System;

namespace Structure.Domain.Notifications
{
    public class Notifier : INotifier
    {
        private readonly Lazy<INotificationCollection> notifications;

        public Notifier(Lazy<INotificationCollection> notifications)
        {
            this.notifications = notifications;
        }

        public INotificationCollection Notifications
        {
            get { return notifications.Value; }
        }
        
        public void Notify(string message)
        {
            Notify(new Notification(message));
        }

        public void Notify(string message, string member)
        {
            Notify(new Notification(message, member));
        }

        public void Notify(INotification notification)
        {
            Notifications.Add(notification);
        }
    }
}
