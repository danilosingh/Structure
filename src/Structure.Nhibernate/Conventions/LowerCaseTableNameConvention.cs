using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Structure.Nhibernate.Mapping;

namespace Structure.Nhibernate.Conventions
{
	public class LowerCaseTableNameConvention : IClassConvention, IClassConventionAcceptance, 
		IJoinedSubclassConvention, IJoinedSubclassConventionAcceptance
	{
		public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
		{
			criteria.Expect(c => c.TableName, Is.Not.NonDefaultIsSet());			
		}

		public void Accept(IAcceptanceCriteria<IJoinedSubclassInspector> criteria)
		{
			criteria.Expect(c => c.TableName, Is.Not.NonDefaultIsSet());
		}

		public void Apply(IClassInstance instance)
		{
			instance.Table(GetTableName(instance));
		}

		public void Apply(IJoinedSubclassInstance instance)
		{
			instance.Table(GetTableName(instance));
		}

		private  string GetTableName(IInspector instance)
		{
			return instance.EntityType.Name.ToLowerInvariant();
		}
	}
}
