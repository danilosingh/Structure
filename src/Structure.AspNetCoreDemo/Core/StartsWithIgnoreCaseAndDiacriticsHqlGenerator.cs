using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using Structure.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Structure.AspNetCoreDemo.Core
{
    public class StartsWithIgnoreCaseAndDiacriticsHqlGenerator : BaseHqlGeneratorForMethod
    {
        public StartsWithIgnoreCaseAndDiacriticsHqlGenerator()
        {
            SupportedMethods = new[]
            {
                ReflectHelper.GetMethodDefinition(() => StringExtensions.StartsWithIgnoreCaseAndDiacritics(null, null))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, System.Linq.Expressions.Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            return treeBuilder.BooleanMethodCall("StartsWithIgnoreCaseAndDiacritics", new List<HqlExpression>()
            {
                visitor.Visit(arguments[0]).AsExpression(),
                visitor.Visit(arguments[1]).AsExpression(),
            });
        }
    }
}
