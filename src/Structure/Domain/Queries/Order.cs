using System;
using System.Linq.Expressions;

namespace Structure.Domain
{
    public class OrderMember<T>
    {
        public Expression<Func<T, object>> MemberExpression { get; set; }
        public OrderType Type { get; set; }

        public OrderMember()
        { }

        public OrderMember(Expression<Func<T, object>> memberExpression, OrderType type)
        {
            MemberExpression = memberExpression;
            Type = type;
        }

        public static OrderMember<T> Create(Expression<Func<T, object>> membroExpression,
            OrderType type = OrderType.Asc)
        {
            return new OrderMember<T>() { MemberExpression = membroExpression, Type = type };
        }

        public static OrderMember<T> Create(string membro,
            OrderType type = OrderType.Asc)
        {
            var parameter = Expression.Parameter(typeof(T), "param");
            var property = typeof(T).GetProperty(membro);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var expression = Expression.Lambda<Func<T, object>>(propertyAccess, parameter);

            return new OrderMember<T>()
            {
                MemberExpression = expression,
                Type = type
            };
        }
    }
}
