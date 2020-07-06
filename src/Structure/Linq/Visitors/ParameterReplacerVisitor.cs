using System.Collections.Generic;
using System.Linq.Expressions;

namespace Structure.Linq.Visitors
{    
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        public ParameterReplacerVisitor(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
            for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                ParameterReplacements.Add(fromParameters[i], toParameters[i]);
        }

        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;

            if (ParameterReplacements.TryGetValue(node, out replacement))
            {
                node = replacement;
            }
            return base.VisitParameter(node);
        }
    }
}
