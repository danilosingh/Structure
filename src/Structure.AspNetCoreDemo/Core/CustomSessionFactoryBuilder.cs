using Microsoft.Extensions.Options;
using NHibernate.Cfg;
using Structure.Data.Filtering;
using Structure.Nhibernate;

namespace Structure.AspNetCoreDemo.Core
{
    public class CustomSessionFactoryBuilder : FluentSessionFactoryBuilder
    {
        public CustomSessionFactoryBuilder(
            IOptions<NhibernateOptions> options,
            IPersistenceConfigurerProvider persistenceConfigurerProvider,
            IDataFilterHandler dataFilterHandler) :
            base(options, persistenceConfigurerProvider, dataFilterHandler)
        { }

        protected override void OnConfigurationExposed(Configuration config)
        {
            base.OnConfigurationExposed(config);
            config.LinqToHqlGeneratorsRegistry<CustomLinqtoHqlGeneratorsRegistry>();
        }
    }
}
