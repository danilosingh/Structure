using Microsoft.Extensions.Options;
using Structure.MultiTenancy;
using System;

namespace Structure.Session
{
    public abstract class AppSessionBase : IAppSession
    {
        public abstract Guid? TenantId { get; }
        public abstract Guid? UserId { get; }
        public virtual MultiTenancySides MultiTenancySide
        {
            get
            {
                return MultiTenancy.IsEnabled && TenantId == null ? MultiTenancySides.Host : MultiTenancySides.Tenant;
            }
        }

        public MultiTenancyOptions MultiTenancy { get; }

        protected AppSessionBase(IOptions<MultiTenancyOptions> multiTenancy)
        {
            MultiTenancy = multiTenancy.Value;
        }
    }
}