using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public interface IUserTenantStore<TUser> 
        where TUser : class
    {
        Task<Guid?> GetTenantIdAsync(TUser user, CancellationToken cancellationToken);
        Task SetTenantIdAsync(TUser user, Guid? tenantId, CancellationToken cancellationToken);
    }
}
