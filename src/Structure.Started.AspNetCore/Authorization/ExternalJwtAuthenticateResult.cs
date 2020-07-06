using System;

namespace Structure.Started.AspNetCore.Authorization
{
    public class ExternalJwtAuthenticateResult<TExternalUser>
    {
        public Guid? TenantId { get; set; }
        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }
        public int ExpireInSeconds { get; set; }
        public bool WaitingForActivation { get; set; }
        //TODO: ignore null
        public string RefreshToken { get; set; }
        public TExternalUser User { get; set; }
    }

    public class ExternalJwtAuthenticateResult : ExternalJwtAuthenticateResult<ExternalJwtAuthenticateUserResult>
    {
    }

    public class ExternalJwtAuthenticateUserResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string[] Roles { get; set; }
        public string[] Permissions { get; set; }
    }
}
