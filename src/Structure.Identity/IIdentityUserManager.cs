using Structure.MultiTenancy;
using Structure.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public interface IIdentityUserManager<TUser>
    {
        bool SupportsUserPhoneNumber { get; }
        bool SupportsUserEmail { get; }
        bool SupportsUserRole { get; }
        bool SupportsUserTenant { get; }
        Task<string> GetUserIdAsync(TUser user);
        Task<Guid?> GetTenantIdAsync(TUser user);
        Task<string> GetUserNameAsync(TUser user);
        Task<string> GetNameAsync(TUser user);
        Task<string> GetEmailAsync(TUser user);
        Task<string> GetPhoneNumberAsync(TUser user);
        Task<IList<string>> GetRolesAsync(TUser user);
        Task<IList<string>> GetPermissions(TUser user);
        Task<IList<string>> GetPermissions(string userId);
        Task<bool> IsGrantedAsync(string tenantId, string userId, string permissionName, CancellationToken cancellationToken, MultiTenancySides? multiTenancySide = null);
        Task<bool> IsGrantedAsync(string tenantId, string userId, Permission permission, CancellationToken cancellationToken, MultiTenancySides? multiTenancySide = null);
    }
}
