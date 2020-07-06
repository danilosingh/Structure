using System;

namespace Structure.MultiTenancy
{
    [Flags]
    public enum MultiTenancySides
    {
        Tenant = 1,
        Host = 2
    }
}
