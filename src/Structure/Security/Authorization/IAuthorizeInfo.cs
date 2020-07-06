namespace Structure.Security.Authorization
{
    public interface IAuthorizeInfo
    {
        string[] Permissions { get; set; }

        bool RequireAll { get; set; }
        string Area { get; set; }
    }
}
