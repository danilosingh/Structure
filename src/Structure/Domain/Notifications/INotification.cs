namespace Structure.Domain.Notifications
{
    public interface INotification
    {        
        string Message { get; set; }
        string Path { get; set; }
    }
}
