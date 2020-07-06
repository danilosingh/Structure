using Structure.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Structure.Linq
{
    public static class ExpressionBuilder
    {
        private static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        private static readonly MethodInfo startsWithIgnoreCaseMethod = typeof(StringExtensions).GetMethod("StartsWithIgnoreCase", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        private static readonly MethodInfo startsWithIgnoreCaseAndDiacriticsMethod = typeof(StringExtensions).GetMethod("StartsWithIgnoreCaseAndDiacritics", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        static ExpressionBuilder()
        { }

        public static Expression<Func<T, bool>> AndAlso<T>(params ExpressionInfo[] infos)
        {
            return Create<T>(infos, BinaryOperator.AndAlso);
        }

        public static Expression<Func<T, bool>> OrElse<T>(params ExpressionInfo[] infos)
        {
            return Create<T>(infos, BinaryOperator.OrElse);
        }

        private static Expression<Func<T, bool>> Create<T>(ExpressionInfo[] infos, BinaryOperator binaryOperator)
        {
            var param = Expression.Parameter(typeof(T), "parm");
            var exp = BuildExpression(param, infos, binaryOperator);
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public static Expression<Func<bool>> Create(Type type, ExpressionInfo[] infos, BinaryOperator binaryOperator)
        {
            var param = Expression.Parameter(type, "parm");
            var exp = BuildExpression(param, infos, binaryOperator);
            return Expression.Lambda<Func<bool>>(exp, param);
        }

        private static Expression BuildExpression(ParameterExpression param, ExpressionInfo[] infos, BinaryOperator binaryOperator)
        {
            Expression exp = null;
            var infosCopy = infos.ToList();

            if (infosCopy.Count == 1)
            {
                exp = GetExpression(param, infos[0]);
            }
            else if (infosCopy.Count == 2)
            {
                exp = CreateBinaryExpression(param, infos[0], infos[1], binaryOperator);
            }
            else
            {
                while (infosCopy.Count > 0)
                {
                    var f1 = infos[0];
                    var f2 = infos[1];

                    if (exp != null)
                    {
                        exp = binaryOperator == BinaryOperator.OrElse ?
                            Expression.OrElse(exp, CreateBinaryExpression(param, infos[0], infos[1], binaryOperator)) :
                            Expression.AndAlso(exp, CreateBinaryExpression(param, infos[0], infos[1], binaryOperator));
                    }
                    else
                    {
                        exp = CreateBinaryExpression(param, infos[0], infos[1], binaryOperator);
                    }

                    infosCopy.Remove(f1);
                    infosCopy.Remove(f2);

                    if (infosCopy.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression(param, infos[0]));
                        infosCopy.RemoveAt(0);
                    }
                }
            }

            return exp;
        }

        private static Expression GetExpression(ParameterExpression param, ExpressionInfo info)
        {
            Expression body = param;

            foreach (var prop in info.Member.Split('.'))
            {
                body = Expression.PropertyOrField(body, prop);
            }

            MemberExpression member = body as MemberExpression;
            ConstantExpression valueConstantExpression = Expression.Constant(info.Value);

            switch (info.Operator)
            {
                case ExpressionOperator.Equals:
                    return Expression.Equal(member, valueConstantExpression);

                case ExpressionOperator.Contains:
                    return Expression.Call(member, containsMethod, valueConstantExpression);

                case ExpressionOperator.GreaterThan:
                    return Expression.GreaterThan(member, valueConstantExpression);

                case ExpressionOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, valueConstantExpression);

                case ExpressionOperator.LessThan:
                    return Expression.LessThan(member, valueConstantExpression);

                case ExpressionOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, valueConstantExpression);

                case ExpressionOperator.StartsWith:
                    return Expression.Call(member, startsWithMethod, valueConstantExpression);

                case ExpressionOperator.StartsWithIgnoreCase:
                    return Expression.Call(null, startsWithIgnoreCaseMethod, member, valueConstantExpression);
                
                case ExpressionOperator.StartsWithIgnoreCaseAndDiacritics:
                    return Expression.Call(null, startsWithIgnoreCaseAndDiacriticsMethod, member, valueConstantExpression);

                case ExpressionOperator.EndsWith:
                    return Expression.Call(member, endsWithMethod, valueConstantExpression);
            }

            return null;
        }

        private static BinaryExpression CreateBinaryExpression(ParameterExpression param, ExpressionInfo firstExpression, ExpressionInfo secondExpression, BinaryOperator binaryOperator)
        {
            return binaryOperator == BinaryOperator.OrElse ?
                Expression.OrElse(GetExpression(param, firstExpression), GetExpression(param, secondExpression)) :
                Expression.AndAlso(GetExpression(param, firstExpression), GetExpression(param, secondExpression));
        }
    }

    public class ExpressionInfo
    {
        public string Member { get; set; }
        public ExpressionOperator Operator { get; set; }
        public object Value { get; set; }

        public ExpressionInfo(string member, ExpressionOperator @operator, object value)
        {
            Member = member;
            Operator = @operator;
            Value = value;
        }
    }

    public enum BinaryOperator
    {
        AndAlso,
        OrElse
    }
}


