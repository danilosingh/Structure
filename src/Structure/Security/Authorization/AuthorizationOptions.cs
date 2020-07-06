namespace Structure.Security.Authorization
{
    public class AuthorizationOptions
    {
        public AuthorizationRoleOptions Role { get; set; }
        
        public AuthorizationOptions()
        {
            Role = new AuthorizationRoleOptions();
        }
    }
}
