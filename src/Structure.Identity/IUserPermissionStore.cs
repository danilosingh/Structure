using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public interface IUserPermissionStore<TUserPermission>
        where TUserPermission : class
    {
        Task<List<TUserPermission>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    }
}
