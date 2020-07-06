using Structure.Extensions;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structure.Nhibernate.Extensions.Linq
{
    public class InGenerator : BaseHqlGeneratorForMethod
    {
        public InGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition(() => ObjectExtesions.In(null, (object[]) null)),
                ReflectHelper.GetMethodDefinition(() => ObjectExtesions.In<object>(null, (IQueryable<object>) null)),
                ReflectHelper.GetMethodDefinition(() => ObjectExtesions.NotIn<object>(null, (object[]) null)),
                ReflectHelper.GetMethodDefinition(() => ObjectExtesions.NotIn<object>(null, (IQueryable<object>) null))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, System.Linq.Expressions.Expression targetObject, ReadOnlyCollection<System.Linq.Expressions.Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            var value = visitor.Visit(arguments[0]).AsExpression();
            HqlTreeNode inClauseNode;

            if (arguments[1] is ConstantExpression)
                inClauseNode = BuildFromArray((Array)((ConstantExpression)arguments[1]).Value, treeBuilder);
            else
                inClauseNode = BuildFromExpression(arguments[1], visitor);

            HqlTreeNode inClause = treeBuilder.In(value, inClauseNode);

            if (method.Name == "NotIn")
                inClause = treeBuilder.BooleanNot((HqlBooleanExpression)inClause);

            return inClause;
        }

        private HqlTreeNode BuildFromExpression(System.Linq.Expressions.Expression expression, IHqlExpressionVisitor visitor)
        {
            return visitor.Visit(expression).AsExpression();
        }

        private HqlTreeNode BuildFromArray(Array valueArray, HqlTreeBuilder treeBuilder)
        {
            var elementType = valueArray.GetType().GetElementType();

            if (!elementType.IsValueType && elementType != typeof(string))
                throw new ArgumentException("Only primitives and strings can be used");

            Type enumUnderlyingType = elementType.IsEnum ? Enum.GetUnderlyingType(elementType) : null;
            var variants = new HqlExpression[valueArray.Length];

            for (int index = 0; index < valueArray.Length; index++)
            {
                var variant = valueArray.GetValue(index);
                var val = variant;

                if (elementType.IsEnum)
                    val = Convert.ChangeType(variant, enumUnderlyingType);

                variants[index] = treeBuilder.Constant(val);
            }

            return treeBuilder.ExpressionSubTreeHolder(variants);
        }
    }
}
