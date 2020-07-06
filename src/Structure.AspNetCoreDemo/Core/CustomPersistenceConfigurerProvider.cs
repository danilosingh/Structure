using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Structure.Nhibernate;
using Structure.Nhibernate.Dialects;

namespace Structure.AspNetCoreDemo.Core
{
    public class CustomPersistenceConfigurerProvider : PersistenceConfigurerProvider
    {
        public CustomPersistenceConfigurerProvider(IConfiguration configuration) : base(configuration)
        { }

        protected override IPersistenceConfigurer GetConfigurer(NhibernateOptions options, string connString)
        {
            if (options.Dialect == NhibernateDialect.PostgreSQL82)
            {
                FluentSessionMappingConfig.Instance.UseCamelCaseNames = true;
                return PostgreSQLConfiguration.PostgreSQL82
                    .Dialect<CustomScePostgreSQL82Dialect>()
                    .DefaultSchema(options.DefaultSchema ?? "public")
                    .ConnectionString(connString);
            }

            return base.GetConfigurer(options, connString);
        }
    }
}
