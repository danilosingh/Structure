using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.MultiTenancy
{
    public interface ICurrentTenantAccessor
    {
        TenantInfo Current { get; set; }
    }
}
