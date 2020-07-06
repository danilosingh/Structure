namespace Structure.Security.Authorization
{
    public class SignInResult<TUser, TTenant>
    {
        public SignInResult(TUser user, TTenant tenant, bool success)
        {
            User = user;
            Tenant = tenant;
            Success = success;
        }

        public TUser User { get; }
        public TTenant Tenant { get; }
        public bool Success { get; set; }
    }
}
