using System.Collections.Generic;
using System.Linq.Expressions;

namespace Structure.Linq.Visitors
{
    public sealed class DelegateConversionVisitor : ExpressionVisitor
    {
        IDictionary<ParameterExpression, ParameterExpression> parametersMap;

        public DelegateConversionVisitor(IDictionary<ParameterExpression, ParameterExpression> parametersMap)
        {
            this.parametersMap = parametersMap;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(this.MapParameter(node));
        }

        public ParameterExpression MapParameter(ParameterExpression source)
        {
            var target = source;
            this.parametersMap.TryGetValue(source, out target);
            return target;
        }
    }
}
