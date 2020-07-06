using Microsoft.Extensions.Options;
using Structure.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Structure.Data.Filtering
{
    public class SoftDeleteFilter : DataFilter<ISoftDelete>
    {
        public SoftDeleteFilter(IOptions<DataFilterOptions> options) : base(options)
        { }

        protected override Expression<Func<ISoftDelete, bool>> CreateExpression()
        {
            return (c) => c.IsDeleted == false;
        }
    }
}
