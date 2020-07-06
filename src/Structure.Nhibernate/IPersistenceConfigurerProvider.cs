using FluentNHibernate.Cfg.Db;

namespace Structure.Nhibernate
{
    public interface IPersistenceConfigurerProvider
    {
        IPersistenceConfigurer GetConfigurer(NhibernateOptions options);
    }
}
