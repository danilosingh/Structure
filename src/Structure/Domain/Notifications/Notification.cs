namespace Structure.Domain.Notifications
{
    public partial class Notification : INotification
    {
        public string Message { get; set; }
        public string Path { get; set; }

        public Notification()
        { }

        public Notification(string message)
        {
            Message = message;
        }

        public Notification(string message, string path) : this(message)
        {
            Path = path;
        }
    }
}
