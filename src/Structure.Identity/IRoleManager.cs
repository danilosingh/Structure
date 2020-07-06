using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public interface IRoleManager<TRole> where TRole : class
    {
        Task<bool> IsGrantedAsync(string roleId, string permissionName, string tenantId = null, CancellationToken cancellationToken = default);
        Task<List<string>> GetPermissions(string roleId, string tenantId = null, CancellationToken cancellationToken = default);
    }
}