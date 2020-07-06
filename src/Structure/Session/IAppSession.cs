using Structure.MultiTenancy;
using System;

namespace Structure.Session
{
    public interface IAppSession
    {
        Guid? TenantId { get; }
        Guid? UserId { get; }
        MultiTenancySides MultiTenancySide { get; }
    }
}
