using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Structure.Started.AspNetCore.Authorization
{
    public class JwtAuthenticationOptions
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; }
        public bool RefreshTokenEnabled { get; set; }

        public SymmetricSecurityKey CreateSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey));
        }

        public SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(CreateSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        }
    }
}
