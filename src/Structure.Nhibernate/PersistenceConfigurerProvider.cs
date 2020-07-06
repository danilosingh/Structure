using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Structure.Nhibernate.Dialects;
using System;

namespace Structure.Nhibernate
{
    public class PersistenceConfigurerProvider : IPersistenceConfigurerProvider
    {
        protected readonly IConfiguration configuration;
        protected const string DefaultConnectionStringName = "default";

        public PersistenceConfigurerProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IPersistenceConfigurer GetConfigurer(NhibernateOptions options)
        {
            var connString = configuration.GetConnectionString(options.ConnectionStringName ?? DefaultConnectionStringName);
            return GetConfigurer(options, connString);
        }

        protected virtual IPersistenceConfigurer GetConfigurer(NhibernateOptions options, string connString)
        {
            switch (options.Dialect)
            {
                case NhibernateDialect.SqlServer2012:
                    return MsSqlConfiguration.MsSql2012
                        .Dialect<SceSqlServer2012Dialect>()
                        .DefaultSchema(options.DefaultSchema ?? "dbo")
                        .ConnectionString(connString);

                case NhibernateDialect.PostgreSQL82:
                    FluentSessionMappingConfig.Instance.UseCamelCaseNames = true;
                    return PostgreSQLConfiguration.PostgreSQL82
                        .Dialect<ScePostgreSQL82Dialect>()
                        .DefaultSchema(options.DefaultSchema ?? "public")
                        .ConnectionString(connString);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
