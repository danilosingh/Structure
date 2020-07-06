using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Structure;
using Structure.AspNetCore;
using Structure.Cfg;
using Structure.Security.Authorization;
using Structure.Started.AspNetCore.Authorization;

namespace Structure.Started.AspNetCore
{
    public class AspNetStartedAddOn : IStructureAspNetAddOn
    {
        public void Configure(IStructureAppBuilder builder)
        {
            builder.Services.AddScoped(
                typeof(IGrantChecker),
                typeof(GrantChecker<,>).MakeGenericType(
                    builder.EntityTypes.User,
                    builder.EntityTypes.Role));

            builder.Services.AddScoped(typeof(SignInManager<>).MakeGenericType(builder.EntityTypes.User));
        }
    }
}
