using Structure.Linq.Visitors;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structure.Helpers
{
    public static class ExpressionHelper
    {
        private static ConcurrentDictionary<MemberInfo, string> displayProperties = new ConcurrentDictionary<MemberInfo, string>();

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            return GetPropertyName<T, object>(expression);
        }

        public static string GetPropertyName<T, TOther>(Expression<Func<T, TOther>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static string GetNestledPropertyName<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            var visitor = new NestedPropertyNameExpressionVisitor();
            visitor.Visit(expression.Body);
            return visitor.GetNestedPropertyName();
        }

        public static object GetValue(LambdaExpression expression)
        {
            var del = expression.Compile();
            return del.DynamicInvoke();
        }

        public static Expression<Func<TInput, bool>> CombineWithAndAlso<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ParameterReplacerVisitor(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        public static TValue GetProperyValue<TSource, TValue>(Expression<Func<TSource, TValue>> propertyExpression, TSource source)
        {
            return propertyExpression.Compile()(source);
        }

        public static void SetProperyValue<TSource, TValue>(Expression<Func<TSource, TValue>> memberLamda, TSource source, TValue value)
        {
            if (memberLamda.Body is MemberExpression memberExpression)
            {
                var property = memberExpression.Member as PropertyInfo;

                if (property != null)
                {
                    property.SetValue(source, value, null);
                }
            }
        }

        public static Expression<Func<TInput, bool>> CombineWithOrElse<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ParameterReplacerVisitor(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        public static Expression<Func<T2, TResult>> Convert<T1, T2, TResult>(Expression<Func<T1, TResult>> expr)
        {
            var parametersMap = expr.Parameters
                .Where(pe => pe.Type == typeof(T1))
                .ToDictionary(pe => pe, pe => Expression.Parameter(typeof(T2)));

            var visitor = new DelegateConversionVisitor(parametersMap);
            var newBody = visitor.Visit(expr.Body);

            var parameters = expr.Parameters.Select(visitor.MapParameter);

            return Expression.Lambda<Func<T2, TResult>>(newBody, parameters);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TConstant, TResult>(Expression<Func<T, TConstant, TResult>> predicate, object constant)
        {
            var body = predicate.Body;
            var substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant, typeof(TConstant)));
            var visitedBody = substitutionVisitor.Visit(body).Reduce();
            return Expression.Lambda<Func<T, TResult>>(visitedBody, predicate.Parameters[0]);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TConstant1, TConstant2, TResult>(Expression<Func<T, TConstant1, TConstant2, bool>> predicate, TConstant1 constant1, TConstant2 constant2)
        {
            var body = predicate.Body;
            var substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant1, typeof(TConstant1)));
            var visitedBody = substitutionVisitor.Visit(body).Reduce();
            var newLambda = Expression.Lambda<Func<T, TConstant2, TResult>>(visitedBody, predicate.Parameters[0], predicate.Parameters[2]);
            return ReplaceParameterByConstant<T, TConstant2, TResult>(newLambda, constant2);
        }

        public static Expression<Func<T, TResult>> ReplaceParameterByConstant<T, TResult>(Expression<Func<T, object, bool>> predicate, object constant)
        {
            var body = predicate.Body;
            var substitutionVisitor = new VariableSubstitutionVisitor(predicate.Parameters[1], Expression.Constant(constant, constant.GetType()));
            var visitedBody = substitutionVisitor.Visit(body).Reduce();
            return Expression.Lambda<Func<T, TResult>>(visitedBody, predicate.Parameters[0]);
        }

        public static string GetPropertyDisplayName<T, TMember>(Expression<Func<T, TMember>> propertyExpression)
        {
            var memberInfo = GetMemberInfo(propertyExpression.Body);
            return GetPropertyDisplayName(memberInfo);
        }

        public static string GetPropertyDisplayName(MemberInfo memberInfo)
        {
            if (displayProperties.ContainsKey(memberInfo))
            {
                return displayProperties[memberInfo];
            }

            var attr = TypeHelper.GetAttributeFromMemberInfo<DisplayNameAttribute>(memberInfo);
            var display = attr != null ? attr.DisplayName : memberInfo.Name;
            displayProperties.AddOrUpdate(memberInfo, display, (key, oldValue) => display);
            return display;
        }

        public static Expression<Func<T, TResult>> GetPropertyExpression<T, TResult>(string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "parm");
            var propertyInfo = TypeHelper.GetPropertyInfo<T>(propertyName);

            Expression columnExpr = Expression.Property(param, propertyInfo);

            if (propertyInfo.PropertyType != typeof(TResult))
                columnExpr = Expression.Convert(columnExpr, typeof(TResult));

            return Expression.Lambda<Func<T, TResult>>(columnExpr, param);
        }

        public static Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName)
        {
            return GetPropertyExpression<T, object>(propertyName);
        }

        public static MemberInfo GetMemberInfo(Expression propertyExpression)
        {
            MemberExpression memberExpr = propertyExpression as MemberExpression;

            if (memberExpr == null)
            {
                if (propertyExpression is UnaryExpression unaryExpr && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }

        public static Expression<Func<T, object>> CreateExpressionFromPropertyInfo<T>(PropertyInfo propertyInfo)
        {
            var entityParam = Expression.Parameter(typeof(T), "e");
            Expression columnExpr = Expression.Property(entityParam, propertyInfo);
            return Expression.Lambda<Func<T, object>>(columnExpr, entityParam);
        }
    }
}
