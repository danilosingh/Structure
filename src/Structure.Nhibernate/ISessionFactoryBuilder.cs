using NHibernate;

namespace Structure.Nhibernate
{
    public interface ISessionFactoryBuilder
    {
        ISessionFactory BuildSessionFactory();
    }
}