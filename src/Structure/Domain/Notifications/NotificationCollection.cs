using Structure.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structure.Domain.Notifications
{
    public class NotificationCollection : Collection<INotification>, INotificationCollection
    {
        public bool HasNotifications
        {
            get { return Items.Any(); }
        }

        public NotificationCollection() 
        { }

        public void Add(INotification[] notifications)
        {
            Items.AddRange(notifications);
        }

        public void Add(string message, string path)
        {
            Items.Add(new Notification(message, path));
        }

        public void Add(string message)
        {
            Items.Add(new Notification(message));
        }

        public void Add(INotificationCollection collection)
        {
            Items.AddRange(collection);
        }

        public IReadOnlyList<INotification> ToList()
        {
            return Items.ToList().AsReadOnly();
        }
    }
}
