using System.Collections.Generic;

namespace Structure.Domain.Notifications
{
    public interface INotificationCollection : IEnumerable<INotification>
    {
        bool HasNotifications { get; }

        void Add(INotification notification);
        void Add(INotification[] notifications);
        void Add(string message, string path);
        void Add(string message);
        IReadOnlyList<INotification> ToList();
    }
}
