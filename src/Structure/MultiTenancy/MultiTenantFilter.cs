using Microsoft.Extensions.Options;
using Structure.Data.Filtering;
using Structure.Domain.Entities;
using Structure.Extensions;
using System;
using System.Linq.Expressions;

namespace Structure.MultiTenancy
{
    public class MultiTenantFilter : DataFilter<IMultiTenant>
    {
        private readonly ICurrentTenant currentTenant;

        public MultiTenantFilter(IOptions<DataFilterOptions> options, ICurrentTenant currentTenant) : base(options)
        {
            this.currentTenant = currentTenant;
        }

        protected override bool CanEnable()
        {
            return !currentTenant.Id.IsNullOrEmpty();
        }

        protected override Expression<Func<IMultiTenant, bool>> CreateExpression()
        {
            return (c) => c.TenantId == null || c.TenantId == currentTenant.Id;
        }
    }
}
