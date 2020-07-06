﻿using System;

namespace Structure.MultiTenancy
{
    public class TenantResolveContext : ITenantResolveContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public string TenantIdOrName { get; set; }

        public bool Handled { get; set; }

        public bool HasResolvedTenantOrHost()
        {
            return Handled || TenantIdOrName != null;
        }

        public TenantResolveContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
