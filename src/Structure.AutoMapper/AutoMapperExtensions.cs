using AutoMapper;
using Structure.Domain.Entities;
using Structure.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Structure.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> SetNullInEntitiesEmpty<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping,
            params Expression<Func<TDestination, object>>[] expressions)
        {
            return mapping.AfterMap((s, d) =>
            {
                foreach (var item in expressions)
                {
                    if (ExpressionHelper.GetProperyValue(item, d) is IEntity entity && !entity.HasIdentifier())
                    {
                        ExpressionHelper.SetProperyValue(item, d, null);
                    }
                }
            });
        }

        public static IMappingExpression<TSource, TDestination> ForPath<TSource, TDestination, TMember>(this IMappingExpression<TSource, TDestination> mapping, string memberName, Action<IPathConfigurationExpression<TSource, TDestination, TMember>> memberOptions)
        {
            return mapping.ForPath(ExpressionHelper.GetPropertyExpression<TDestination, TMember>(memberName), memberOptions);
        }

        public static IMappingExpression<TDestination, TSource> ReserseMapWithIgnoreEntitiesEmpty<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
        {
            return map.ReverseMap().SetNullInEntitiesEmpty(TypeHelper.GetPropertiesOf<IEntity>(typeof(TSource))
                .Select(c => ExpressionHelper.CreateExpressionFromPropertyInfo<TSource>(c))
                .ToArray());
        }

        public static void OnlyNumbers<TSource, TDestination>(this IMemberConfigurationExpression<TSource, TDestination, string> memberConfigurationExpression)
        {
            memberConfigurationExpression.AddTransform((value) => StringHelper.OnlyNumbers(value));
        }
    }
}
