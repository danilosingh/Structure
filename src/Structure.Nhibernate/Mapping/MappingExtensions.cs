using FluentNHibernate;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using Structure.Helpers;
using Structure.Nhibernate.Conventions.AcceptanceCriteria;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Structure.Nhibernate.Mapping
{
    public static class MappingExtensions
    {
        public static IdentityPart Sequence<TEntity>(this IdentityGenerationStrategyBuilder<IdentityPart> generateByPart)
        {
            return generateByPart.Sequence("Seq" + typeof(TEntity).Name);
        }

        public static DiscriminatorPart DiscriminateSubClassesOnColumn<T>(this ClassMap<T> classMap, Expression<Func<T, object>> memberExpression)
        {
            return classMap.DiscriminateSubClassesOnColumn(ExpressionHelper.GetPropertyName(memberExpression));
        }

        public static ManyToOnePart<TOther> Indexable<TOther>(this ManyToOnePart<TOther> manyToOnePart)
        {
            return manyToOnePart.Index("___Indexable");
        }

        public static PropertyPart Indexable(this PropertyPart propertyPart)
        {
            return propertyPart.Index("___Indexable");
        }

        public static bool NonDefaultIsSet(this IInspector inspector, Member property)
        {
            var layeredValues = TypeHelper.GetNestedFieldValue(inspector, "mapping.attributes.layeredValues") as AttributeLayeredValues;
            return layeredValues[property.Name].Keys.Any(c => c != Layer.Defaults);
        }

        public static IAcceptanceCriterion NonDefaultIsSet(this InverseIs _)
        {
            return new NonDefaultSetCriterion(true);
        }
    }
}
