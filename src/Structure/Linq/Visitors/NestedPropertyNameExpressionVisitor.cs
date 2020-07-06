using System.Linq.Expressions;

namespace Structure.Linq.Visitors
{
    public class NestedPropertyNameExpressionVisitor : ExpressionVisitor
    {
        private string nestedPropertyName = string.Empty;

        protected override Expression VisitMember(MemberExpression node)
        {
            nestedPropertyName = node.Member.Name + "." + nestedPropertyName;
            return base.VisitMember(node);
        }

        public string GetNestedPropertyName()
        {
            return nestedPropertyName.Remove(nestedPropertyName.Length - 1);
        }
    }
}
