using NHibernate.Linq.Functions;
using Structure.Nhibernate.Extensions.Linq;

namespace Structure.AspNetCoreDemo.Core
{
    public class CustomLinqtoHqlGeneratorsRegistry : ExtendedLinqtoHqlGeneratorsRegistry
    {
        public CustomLinqtoHqlGeneratorsRegistry()
        {
            this.Merge(new StartsWithIgnoreCaseAndDiacriticsHqlGenerator());
        }
    }
}
