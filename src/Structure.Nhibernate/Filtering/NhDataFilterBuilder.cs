using NHibernate;
using NHibernate.Type;
using Structure.Data.Filtering;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Structure.Nhibernate.Filtering
{
    public static class NhDataFilterBuilder
    {
        public static NhDataFilter Build(IDataFilter dataFilter)
        {
            var i = 1;
            return new NhDataFilter(dataFilter, Recurse(ref i, dataFilter.ToExpression()));
        }

        private static NhFilterDefinition Recurse(ref int i, Expression expression, string prefix = null, string postfix = null, bool isBinary = false)
        {
            if (expression is UnaryExpression unaryExpression)
            {
                return FromUnaryExpression(ref i, unaryExpression);
            }

            if (expression is BinaryExpression binaryExpression)
            {
                return FromBinaryExpression(ref i, binaryExpression);
            }

            if (expression is ConstantExpression constantExpression)
            {
                return FromConstantExpression(ref i, prefix, postfix, constantExpression, false);
            }

            if (expression is MemberExpression memberExpression)
            {
                return FromMemberExpression(ref i, expression, prefix, postfix, memberExpression, isBinary);
            }

            if (expression is MethodCallExpression methodCallExpression)
            {
                return FromMethodCallExpression(ref i, methodCallExpression);
            }

            if (expression is InvocationExpression invocationExpression)
            {
                return Recurse(ref i, invocationExpression.Expression, prefix, postfix);
            }

            if (expression is LambdaExpression lambdaExpression)
            {
                return Recurse(ref i, lambdaExpression.Body, prefix, postfix);
            }

            throw new Exception("Unsupported expression: " + expression.GetType().Name);
        }

        private static NhFilterDefinition FromMethodCallExpression(ref int i, MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method == typeof(string).GetMethod("Contains", new[] { typeof(string) }))
            {
                return NhFilterDefinition.Concat(
                    Recurse(ref i, methodCallExpression.Object), "LIKE",
                    Recurse(ref i, methodCallExpression.Arguments[0], prefix: "%", postfix: "%")
                );
            }

            if (methodCallExpression.Method == typeof(string).GetMethod("StartsWith", new[] { typeof(string) }))
            {
                return NhFilterDefinition.Concat(
                    Recurse(ref i, methodCallExpression.Object), "LIKE",
                    Recurse(ref i, methodCallExpression.Arguments[0], postfix: "%")
                );
            }

            if (methodCallExpression.Method == typeof(string).GetMethod("EndsWith", new[] { typeof(string) }))
            {
                return NhFilterDefinition.Concat(
                    Recurse(ref i, methodCallExpression.Object), "LIKE",
                    Recurse(ref i, methodCallExpression.Arguments[0], prefix: "%")
                 );
            }

            if (methodCallExpression.Method.Name == "Contains")
            {
                Expression collection;
                Expression property;
                if (methodCallExpression.Method.IsDefined(typeof(ExtensionAttribute)) && methodCallExpression.Arguments.Count == 2)
                {
                    collection = methodCallExpression.Arguments[0];
                    property = methodCallExpression.Arguments[1];
                }
                else if (!methodCallExpression.Method.IsDefined(typeof(ExtensionAttribute)) && methodCallExpression.Arguments.Count == 1)
                {
                    collection = methodCallExpression.Object;
                    property = methodCallExpression.Arguments[0];
                }
                else
                {
                    throw new Exception("Unsupported method call: " + methodCallExpression.Method.Name);
                }

                var values = (IEnumerable)GetValue(collection);
                return NhFilterDefinition.Concat(Recurse(ref i, property), "IN", NhFilterDefinition.FromCollection(ref i, values, GetNhType(values.GetType())));
            }

            if (methodCallExpression.Type == typeof(string))
            {
                return NhFilterDefinition.FromParameter(i++, GetValue(methodCallExpression), null);
            }

            throw new Exception("Unsupported method call: " + methodCallExpression.Method.Name);
        }

        private static NhFilterDefinition FromMemberExpression(ref int i, Expression expression, string prefix, string postfix, MemberExpression memberExpression, bool isBinary = false)
        {
            if (IsMethodOrConstant(memberExpression))
            {
                return FromConstantExpression(ref i, prefix, postfix, Expression.Constant(GetValue(memberExpression), memberExpression.Type), true);
            }

            if (memberExpression.Expression is MemberExpression)
            {
                return FromMemberExpression(ref i, memberExpression, prefix, postfix, memberExpression.Expression as MemberExpression, isBinary);
            }

            if (memberExpression.Member is PropertyInfo)
            {
                var property = (PropertyInfo)memberExpression.Member;
                var colName = property.Name;

                if (!isBinary && memberExpression.Type == typeof(bool))
                {
                    return NhFilterDefinition.Concat(NhFilterDefinition.FromCondition(colName), "=", NhFilterDefinition.FromParameter(i++, true, GetNhType(memberExpression.Type)));
                }

                return NhFilterDefinition.FromCondition(colName);
            }

            if (memberExpression.Member is FieldInfo)
            {
                var value = GetValue(memberExpression);
                if (value is string)
                {
                    value = prefix + (string)value + postfix;
                }
                return NhFilterDefinition.FromParameter(i++, value, GetNhType(memberExpression.Type));
            }

            throw new Exception($"Expression does not refer to a property or field: {expression}");
        }

        private static IType GetNhType(Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }

            if (type == typeof(bool))
            {
                return NHibernateUtil.Boolean;
            }
            else if (type == typeof(string))
            {
                return NHibernateUtil.String;
            }
            else if (type == typeof(int))
            {
                return NHibernateUtil.Int32;
            }
            if (type == typeof(Guid))
            {
                return NHibernateUtil.Guid;
            }
            else
            {
                return NHibernateUtil.Object;
            }
        }

        private static bool IsMethodOrConstant(MemberExpression memberExpression)
        {
            var expression = memberExpression.Expression;

            while (expression is MemberExpression)
            {
                expression = (expression as MemberExpression).Expression;
            }

            return (expression is MethodCallExpression) || (expression is ConstantExpression);
        }

        private static NhFilterDefinition FromConstantExpression(ref int i, string prefix, string postfix, ConstantExpression constantExpression, bool isMember)
        {
            var value = constantExpression.Value;

            if (value == null && !isMember)
            {
                return NhFilterDefinition.FromHql("NULL");
            }

            return NhFilterDefinition.FromParameter(i++, constantExpression.Value, GetNhType(constantExpression.Type));
        }

        private static NhFilterDefinition FromUnaryExpression(ref int i, UnaryExpression unaryExpression)
        {
            return NhFilterDefinition.FromUnary(NodeTypeToString(unaryExpression.NodeType), Recurse(ref i, unaryExpression.Operand));
        }

        private static NhFilterDefinition FromBinaryExpression(ref int i, BinaryExpression binaryExpression)
        {
            var left = Recurse(ref i, binaryExpression.Left, isBinary: true);
            var right = Recurse(ref i, binaryExpression.Right);
            var operand = NodeTypeToString(binaryExpression.NodeType, right?.Condition == "NULL");
            return NhFilterDefinition.Concat(left, operand, right);
        }

        private static object GetValue(Expression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static string NodeTypeToString(ExpressionType nodeType, bool isNull = false)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                    return "&";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Equal:
                    return isNull ? "IS" : "=";
                case ExpressionType.ExclusiveOr:
                    return "^";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Negate:
                    return "-";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return "|";
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.Convert:
                    return "";
            }
            throw new Exception($"Unsupported node type: {nodeType}");
        }
    }
}
