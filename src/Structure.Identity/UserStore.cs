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
    public class UserStore<TUser, TRole> :
       IUserStore<TUser>,
       IUserPasswordStore<TUser>,
       IUserEmailStore<TUser>,
       IQueryableUserStore<TUser>,
       IUserLockoutStore<TUser>,
       IUserPhoneNumberStore<TUser>

       where TUser : IdentityUser
       where TRole : IdentityRole
    {
        protected readonly IDataContext persistenceContext;

        public IQueryable<TUser> Users
        {
            get { return persistenceContext.Query<TUser>(); }
        }

        public IQueryable<TRole> Roles
        {
            get { return persistenceContext.Query<TRole>(); }
        }

        public UserStore(IDataContext dataContext)
        {
            this.persistenceContext = dataContext;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            await persistenceContext.CreateAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            await persistenceContext.DeleteAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposed)
        { }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = ConvertIdFromString(userId);
            return await Users.FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        protected virtual Guid ConvertIdFromString(string userId)
        {
            return new Guid(userId);
        }

        protected virtual string ConvertIdToString(Guid userId)
        {
            if (object.Equals(userId, default(Guid)))
            {
                return null;
            }

            return userId.ToString();
        }

        public virtual Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Users.FirstOrDefaultAsync(c => c.NormalizedUserName == normalizedUserName);
        }

        public virtual Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedUserName);
        }

        public virtual Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ConvertIdToString(user.Id));
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserName);
        }

        public virtual Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public virtual Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await persistenceContext.UpdateAsync(user, cancellationToken);
            return IdentityResult.Success;
        }

        public virtual Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public virtual async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await Users.FirstOrDefaultAsync(c => c.NormalizedEmail == normalizedEmail, cancellationToken);
        }

        public virtual Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Email);
        }

        public virtual Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedEmail);
        }

        public virtual Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            return Task.CompletedTask;
        }

        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public virtual Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnd);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }
    }

    public class UserStore<TUser, TUserRole, TRole> : UserStore<TUser, TRole>, IUserRoleStore<TUser>
        where TUser : IdentityUser
        where TRole : IdentityRole
        where TUserRole : IdentityUserRole<TUser, TRole>, new()
    {

        public IQueryable<TUserRole> UserRoles
        {
            get { return persistenceContext.Query<TUserRole>(); }
        }

        public UserStore(IDataContext persistenceContext) : base(persistenceContext)
        { }

        protected virtual Task<TRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Roles.SingleOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public async Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (role == null)
            {
                throw new InvalidOperationException("Role not found");
            }

            await persistenceContext.CreateAsync(CreateUserRole(user, role), cancellationToken);
        }

        protected virtual TUserRole CreateUserRole(TUser user, TRole role)
        {
            return new TUserRole()
            {
                User = user,
                Role = role
            };
        }

        protected virtual string ConvertRoleIdToString(Guid roleId)
        {
            return roleId.ToString();
        }

        public async Task<IList<string>> GetRoleIdsAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var typedUserId = ConvertIdFromString(userId);

            var query = from userRole in UserRoles
                        join role in Roles on userRole.Role.Id equals role.Id
                        where userRole.User.Id.Equals(typedUserId)
                        select role.Id;

            var roleIds = await query.ToListAsync(cancellationToken);

            return roleIds.Select(c => ConvertRoleIdToString(c)).ToList();
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (roleEntity != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);

                if (userRole != null)
                {
                    await persistenceContext.DeleteAsync(userRole);
                }
            }
        }

        protected virtual Task<TUserRole> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            return UserRoles.Where(c => c.User.Id.Equals(userId) && c.Role.Id.Equals(roleId)).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await UserRoles.Where(c => c.Role.Name == roleName)
                .Select(c => c.User)
                .ToListAsync(cancellationToken);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            return UserRoles.AnyAsync(c => c.User == user && c.Role.NormalizedName == roleName);
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userId = user.Id;

            var query = from userRole in UserRoles
                        join role in Roles on userRole.Role.Id equals role.Id
                        where userRole.User.Id.Equals(userId)
                        select role;

            return await query.Select(c => c.Name).ToListAsync(cancellationToken);
        }
    }
}
