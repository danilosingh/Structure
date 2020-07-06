using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Utils;
using Structure.Nhibernate.Mapping;
using System;
using System.Linq.Expressions;

namespace Structure.Nhibernate.Conventions.AcceptanceCriteria
{
    public class NonDefaultSetCriterion : IAcceptanceCriterion
    {
        private readonly bool inverse;

        public NonDefaultSetCriterion(bool inverse)
        {
            this.inverse = inverse;
        }

        public bool IsSatisfiedBy<T>(Expression<Func<T, object>> expression, T inspector) where T : IInspector
        {
            var member = expression.ToMember();
            var result = inspector.NonDefaultIsSet(member);
            return inverse ? !result : result;
        }
    }
}
