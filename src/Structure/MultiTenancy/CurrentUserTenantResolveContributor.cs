using Microsoft.Extensions.DependencyInjection;
using Structure.Session;

namespace Structure.MultiTenancy
{
    public class CurrentUserTenantResolveContributor : ITenantResolveContributor
    {
        public const string ContributorName = "CurrentUser";

        public string Name => ContributorName;

        public void Resolve(ITenantResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

            if (!currentUser.IsAuthenticated)
            {
                return;
            }

            context.Handled = true;
            context.TenantIdOrName = currentUser.TenantId?.ToString();
        }
    }
}
