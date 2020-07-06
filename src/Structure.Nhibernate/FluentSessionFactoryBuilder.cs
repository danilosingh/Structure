using FluentNHibernate.Cfg;
using Microsoft.Extensions.Options;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Structure.Data.Filtering;
using Structure.Nhibernate.Conventions;
using Structure.Nhibernate.Extensions.Linq;
using Structure.Nhibernate.Filtering;
using System;
using System.IO;
using System.Reflection;

namespace Structure.Nhibernate
{
    public class FluentSessionFactoryBuilder : ISessionFactoryBuilder
    {
        private readonly string schemaUpdateFileName = "schemaupdate.sql";
        private readonly IPersistenceConfigurerProvider persistenceConfigurerProvider;
        private readonly IDataFilterHandler dataFilterHandler;
        private readonly NhibernateOptions options;

        public FluentSessionFactoryBuilder(
            IOptions<NhibernateOptions> options,
            IPersistenceConfigurerProvider persistenceConfigurerProvider,
            IDataFilterHandler dataFilterHandler)
        {
            this.persistenceConfigurerProvider = persistenceConfigurerProvider;
            this.dataFilterHandler = dataFilterHandler;
            this.options = options.Value;
        }

        public virtual ISessionFactory BuildSessionFactory()
        {
            var fluentConfig = Fluently.Configure(new NHibernate.Cfg.Configuration());

            ConfigureDatabase(fluentConfig);
            ConfigureMappings(fluentConfig);

            fluentConfig.ExposeConfiguration(c =>
            {
                SchemaUpdate(c);
                OnConfigurationExposed(c);
            });
            return fluentConfig
                .BuildConfiguration()
                .BuildSessionFactory();
        }

        protected virtual void OnConfigurationExposed(NHibernate.Cfg.Configuration config)
        {
            config.LinqToHqlGeneratorsRegistry<ExtendedLinqtoHqlGeneratorsRegistry>();
        }

        protected virtual void SchemaUpdate(NHibernate.Cfg.Configuration config)
        {
            if (!(options.SchemaUpdate?.AllowsInvokeSchemaUpdate() ?? false))
            {
                return;
            }

            if (options.SchemaUpdate.SaveToFile)
            {
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, schemaUpdateFileName);

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                new SchemaUpdate(config).Execute((script) => GenerateSchemaUpdateScriptFile(fileName, script), options.SchemaUpdate.DoUpdate);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, options.SchemaUpdate.DoUpdate);
            }
        }

        protected virtual void GenerateSchemaUpdateScriptFile(string fileName, string script)
        {
            using (var file = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine(script.Trim() + ";");
                    writer.Close();
                }
            }
        }

        protected virtual void ConfigureMappings(FluentConfiguration fluentConfig)
        {
            ConfigureFilters();

            fluentConfig.Mappings(mappings =>
            {
                foreach (var assemblyName in options.MappingAssemblies)
                {
                    var assembly = Assembly.Load(assemblyName);

                    var fluentMappingsContainer = mappings.FluentMappings.AddFromAssembly(assembly);

                    AddFilters(fluentMappingsContainer);
                    AddConventions(assembly, fluentMappingsContainer);
                }
            });
        }

        private void AddFilters(FluentMappingsContainer fluentMappingsContainer)
        {
            foreach (var filter in FluentSessionMappingConfig.Instance.Filters)
            {
                var type = typeof(FluentFilterDefinition<>).MakeGenericType(filter.Type);
                fluentMappingsContainer.Add(type);
            }
        }

        private void ConfigureFilters()
        {
            foreach (var dataFilter in dataFilterHandler.GetFilters())
            {
                FluentSessionMappingConfig.Instance.Filters.Add(dataFilter.ToNhDataFilter());
            }
        }

        protected virtual void AddConventions(Assembly assembly, FluentMappingsContainer fluentMappingsContainer)
        {
            if (options.Dialect == NhibernateDialect.PostgreSQL82)
            {
                fluentMappingsContainer.Conventions.Add<LowerCaseTableNameConvention>();
            }

            fluentMappingsContainer
                .Conventions.Add<ReferenceNameConvention>()
                .Conventions.Add<EntityClassConvention>();
        }

        protected virtual void ConfigureDatabase(FluentConfiguration fluentConfig)
        {
            fluentConfig.Database(persistenceConfigurerProvider.GetConfigurer(options));
        }
    }
}
