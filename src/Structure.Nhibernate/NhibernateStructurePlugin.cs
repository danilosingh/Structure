using Microsoft.Extensions.DependencyInjection;
using Structure.Cfg;
using Structure.Data;
using Structure.Data.Repositories;
using Structure.Domain.Repositories;
using Structure.Linq.Async;
using Structure.Nhibernate.Data;
using Structure.Nhibernate.Linq;

namespace Structure.Nhibernate
{
    public class NhibernateStructurePlugin : IStructurePlugin
    {
        public void Configure(IStructureAppBuilder builder)
        {
            AsyncQueryableExecutor.SetExecutor(new NhAsyncQueryableExecutor());

            builder.Services.AddSingleton<ISessionFactoryBuilder, FluentSessionFactoryBuilder>();
            builder.Services.AddSingleton<IPersistenceConfigurerProvider, PersistenceConfigurerProvider>();
            builder.Services.AddSingleton((c) => c.GetService<ISessionFactoryBuilder>().BuildSessionFactory());
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<ITransactionManager, NhTransactionManager>();
            builder.Services.AddScoped<IDataContext, NhDataContext>();
            builder.Services.AddScoped<INhDataContext, NhDataContext>();
            builder.Services.AddScoped<INhSessionProvider, NhSessionProvider>();
            builder.Services.Configure<NhibernateOptions>(builder.Configuration.GetSection("NHibernate"));
        }
    }
}
