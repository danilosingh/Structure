using Structure.Collections;
using Structure.Collections.Extensions;
using System;
using System.Collections.Generic;

namespace Structure.Security.Authorization
{
    public class PermissionStore : IPermissionStore
    {
        protected readonly IPermissionCollection permissions;
        protected readonly IPermissionRegistration permissionRegistration;

        public PermissionStore(IPermissionRegistration permissionRegistration)
        {
            this.permissionRegistration = permissionRegistration;
            this.permissions = CreateCollection();
            Initialize();
        }

        protected virtual IPermissionCollection CreateCollection()
        {
            return new PermissionCollection();
        }

        public Permission GetByName(string name)
        {
            return permissions.Get(name) ?? throw new Exception("There is no permission with name: " + name);
        }
        public Permission GetOrNullByName(string name)
        {
            return permissions.Get(name);
        }

        public IEnumerable<Permission> GetAll()
        {
            return permissions.All().AsReadOnly();
        }

        protected virtual void Initialize()
        {
            permissionRegistration.Register(permissions);

            if (permissions is IHierarchicalCollection collection)
            {
                collection.Flatten();
            }
        }
    }
}
