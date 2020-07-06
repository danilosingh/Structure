using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public interface IRolePermissionStore<TRolePermission> 
    {
        Task<List<TRolePermission>> GetPermissionsAsync(string roleId, CancellationToken cancellationToken);
    }
}