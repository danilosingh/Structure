using Structure.Threading;
using System;

namespace Structure.MultiTenancy
{
    //TODO: Refector to get tenant
    public class CurrentTenant : ICurrentTenant
    {
        private readonly ICurrentTenantAccessor currentTenantAccessor;

        public Guid? Id
        {
            get { return currentTenantAccessor.Current?.TenantId; }
        }

        public string Name
        {
            get { return currentTenantAccessor.Current?.Name; }
        }

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor)
        {
            this.currentTenantAccessor = currentTenantAccessor;
        }

        public IDisposable Change(Guid? tenantId, string name = null)
        {
            var parentScope = currentTenantAccessor.Current;
            currentTenantAccessor.Current = new TenantInfo(tenantId, name);

            return new DisposeAction(() =>
            {
                currentTenantAccessor.Current = parentScope;
            });
        }
    }
}
