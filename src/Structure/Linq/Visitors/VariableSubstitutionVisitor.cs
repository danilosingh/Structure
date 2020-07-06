using System.Linq.Expressions;

namespace Structure.Linq.Visitors
{
    public class VariableSubstitutionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;
        private readonly ConstantExpression constant;

        public VariableSubstitutionVisitor(ParameterExpression parameter, ConstantExpression constant)
        {
            this.parameter = parameter;
            this.constant = constant;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node == parameter)
            {
                return constant;
            }

            return node;
        }
    }
}
