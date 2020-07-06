using Microsoft.AspNetCore.Identity;
using Structure.Data;
using Structure.Linq.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Identity
{
    public class RoleStore<TRole> : IRoleStore<TRole>
        where TRole : IdentityRole
    {
        protected readonly IDataContext persistenceContext;

        public IQueryable<TRole> Roles
        {
            get { return persistenceContext.Query<TRole>(); }
        }

        public RoleStore(IDataContext persistenceContext)
        {
            this.persistenceContext = persistenceContext;
        }

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await persistenceContext.CreateAsync(role, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            await persistenceContext.DeleteAsync(role, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposed)
        { }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = ConvertIdFromString(roleId);
            return await Roles.FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        protected virtual Guid ConvertIdFromString(string roleId)
        {
            return new Guid(roleId);
        }

        protected virtual string ConvertIdToString(Guid id)
        {
            if (object.Equals(id, default(Guid)))
            {
                return null;
            }

            return id.ToString();
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Roles.FirstOrDefaultAsync(c => c.NormalizedName == normalizedRoleName);
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ConvertIdToString(role.Id));
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await persistenceContext.UpdateAsync(role, cancellationToken);
            return IdentityResult.Success;
        }
    }

    public class RoleStore<TRole, TRolePermission> : RoleStore<TRole>, IRolePermissionStore<TRolePermission>
        where TRole : IdentityRole
        where TRolePermission : IdentityRolePermission
    {
        public IQueryable<TRolePermission> Permissions
        {
            get { return persistenceContext.Query<TRolePermission>(); }
        }

        public RoleStore(IDataContext persistenceContext) : base(persistenceContext)
        { }

        public async Task<List<TRolePermission>> GetPermissionsAsync(string roleId, CancellationToken cancellationToken)
        {
            var id = ConvertIdFromString(roleId);

            return await Permissions
                .Where(c => c.RoleId == id)
                .ToListAsync(); ;
        }
    }
}
