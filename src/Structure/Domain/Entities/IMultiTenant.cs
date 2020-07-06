using System;

namespace Structure.Domain.Entities
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; set; }
    }
}
