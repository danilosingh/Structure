using Structure.Data;
using Structure.Linq.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{


    public class UserPermissionStore<TUser, TUserPermision, TRole> : UserStore<TUser, TRole>, IUserPermissionStore<TUserPermision>
        where TUser : IdentityUser
        where TRole : IdentityRole
        where TUserPermision : IdentityUserPermission
    {
        public UserPermissionStore(IDataContext persistenceContext) : base(persistenceContext)
        {
        }

        public IQueryable<TUserPermision> UserPermissions
        {
            get { return persistenceContext.Query<TUserPermision>(); }
        }

        public async Task<List<TUserPermision>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var typedUserId = ConvertIdFromString(userId);

            return await UserPermissions.Where(c => c.UserId.Equals(typedUserId)).ToListAsync(cancellationToken);
        }
    }

    public class UserPermissionStore<TUser, TUserRole, TUserPermision, TRole> : UserStore<TUser, TUserRole, TRole>, IUserPermissionStore<TUserPermision>

       where TUser : IdentityUser
       where TRole : IdentityRole
       where TUserRole : IdentityUserRole<TUser, TRole>, new()
       where TUserPermision : IdentityUserPermission
    {
        public UserPermissionStore(IDataContext persistenceContext) : base(persistenceContext)
        {
        }

        public IQueryable<TUserPermision> UserPermissions
        {
            get { return persistenceContext.Query<TUserPermision>(); }
        }

        public async Task<List<TUserPermision>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var typedUserId = ConvertIdFromString(userId);
            return await UserPermissions.Where(c => c.UserId.Equals(typedUserId)).ToListAsync(cancellationToken);
        }
    }
}
