using NHibernate.Linq.Functions;

namespace Structure.Nhibernate.Extensions.Linq
{
    public class ExtendedLinqtoHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public ExtendedLinqtoHqlGeneratorsRegistry()
        {
            this.Merge(new InGenerator());
        }
    }
}
