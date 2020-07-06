
using Structure.Collections.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Security.Authorization
{
    public static class PermissionCheckerExtensions
    {
        public static async Task<bool> IsGrantedAsync(this IGrantChecker permissionChecker, bool requiresAll, string[] permissionNames, CancellationToken cancellationToken)
        {
            return requiresAll ? await permissionChecker.IsGrantedAllAsync(permissionNames, cancellationToken) : await permissionChecker.IsGrantedAnyAsync(permissionNames, cancellationToken);
        }

        private static async Task<bool> IsGrantedAnyAsync(this IGrantChecker permissionChecker, string[] permissionNames, CancellationToken cancellationToken)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            foreach (var item in permissionNames)
            {
                if (await permissionChecker.IsGrantedAsync(item, cancellationToken))
                {
                    return true;
                }
            }

            return false;
        }

        private static async Task<bool> IsGrantedAllAsync(this IGrantChecker permissionChecker, string[] permissionNames, CancellationToken cancellationToken)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            foreach (var item in permissionNames)
            {
                if(!await permissionChecker.IsGrantedAsync(item, cancellationToken))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
