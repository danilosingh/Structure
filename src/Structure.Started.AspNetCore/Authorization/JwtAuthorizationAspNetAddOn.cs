using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Structure.AspNetCore;
using Structure.Cfg;
using Structure.Security.Authorization;
using System;

namespace Structure.Started.AspNetCore.Authorization
{
    public class JwtAuthorizationAspNetAddOn : IStructureAspNetAddOn
    {
        private readonly JwtAuthenticationOptions options;
        private readonly Action<JwtBearerOptions> configureJwtBearer;

        public JwtAuthorizationAspNetAddOn(JwtAuthenticationOptions options, Action<JwtBearerOptions> configureJwtBearer = null)
        {
            this.options = options;
            this.configureJwtBearer = configureJwtBearer;
        }

        public void Configure(IStructureAppBuilder builder)
        {
            //TODO: reactor AddAuthentication to allow mutiples authentication schemes

            builder.Services
                .AddAuthentication(c =>
                {
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
                {
                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                    opts.Audience = options.Audience;
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = options.CreateSymmetricSecurityKey(),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    configureJwtBearer?.Invoke(opts);
                });

            builder.Services.AddScoped(
               typeof(IGrantChecker),
               typeof(GrantChecker<,>).MakeGenericType(
                   builder.EntityTypes.User,
                   builder.EntityTypes.Role));

            builder.Services.AddScoped(typeof(SignInManager<>).MakeGenericType(builder.EntityTypes.User));

            builder.Services.AddScoped(
               typeof(IRefreshTokenStore<>),
               typeof(RefreshTokenStore<>));

        }
    }
}
