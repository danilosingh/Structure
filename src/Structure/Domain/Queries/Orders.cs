using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Structure.Domain
{
    public class Order<T> : Collection<OrderMember<T>>
    {        
        public void Add(Expression<Func<T, object>> member)
        {
            Items.Add(OrderMember<T>.Create(member, OrderType.Asc));
        }

        public void Add(string memberName)
        {
            Items.Add(OrderMember<T>.Create(memberName, OrderType.Asc));
        }

        public void AddDescending(Expression<Func<T, object>> member)
        {
            Items.Add(OrderMember<T>.Create(member, OrderType.Desc));
        }

        public void AddDescending(string memberName)
        {            
              Items.Add(OrderMember<T>.Create(memberName, OrderType.Desc));
        }

        public IQueryable<T> ApplyOrder(IQueryable<T> query)
        {
            if (Items.Count == 0)
            {
                return query;
            }

            IOrderedQueryable<T> orderedQueryable = Items[0].Type == OrderType.Asc ?
                query.OrderBy(Items[0].MemberExpression) :
                query.OrderByDescending(Items[0].MemberExpression);

            for (int i = 1; i < Items.Count; i++)
            {
                orderedQueryable = Items[i].Type == OrderType.Asc ?
                    orderedQueryable.ThenBy(Items[i].MemberExpression) :
                    orderedQueryable.ThenByDescending(Items[i].MemberExpression);
            }

            return orderedQueryable;
        }
    }

    public static class Order
    {
        public static Order<T> Asc<T>(params Expression<Func<T, object>>[] members)
        {
            Order<T> order = new Order<T>();

            foreach (var member in members)
            {
                order.Add(member);
            }

            return order;
        }


        public static Order<T> Desc<T>(params Expression<Func<T, object>>[] members)
        {
            Order<T> order = new Order<T>();

            foreach (var member in members)
            {
                order.AddDescending(member);
            }

            return order;
        }

        public static Order<T> Empty<T>()
        {
            return new Order<T>();
        }
    }
}
