using Structure.Identity;
using Structure.Security.Authorization;
using Structure.Session;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Started.AspNetCore.Authorization
{
    public class GrantChecker<TUser, TRole> : IGrantChecker
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        private readonly IIdentityUserManager<TUser> userManager;
        private readonly IPermissionStore permissionStore;
        private readonly IAppSession session;

        public GrantChecker(
            IAppSession session,
            IIdentityUserManager<TUser> userManager,
            IPermissionStore permissionStore)
        {
            this.userManager = userManager;
            this.permissionStore = permissionStore;
            this.session = session;
        }

        public async Task<bool> IsGrantedAsync(string permissionName, CancellationToken cancellationToken)
        {
            return await IsGrantedAsync(session.UserId?.ToString(), permissionStore.GetByName(permissionName), cancellationToken);
        }

        private async Task<bool> IsGrantedAsync(string userId, Permission permission, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if(await userManager.IsGrantedAsync(null, userId, permission, cancellationToken: cancellationToken))
            {
                return true;
            }
       
            return false;
        }
    }
}