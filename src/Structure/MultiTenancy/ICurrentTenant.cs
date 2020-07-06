using System;

namespace Structure.MultiTenancy
{
    public interface ICurrentTenant
    {
        Guid? Id { get; }
        string Name { get; }

        IDisposable Change(Guid? tenantId, string name = null);
    }
}
