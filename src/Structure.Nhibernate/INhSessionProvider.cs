using NHibernate;
using System;

namespace Structure.Nhibernate
{
    public interface INhSessionProvider : IDisposable
    {
        ISession Session { get; }
        IStatelessSession StatelessSession { get; }
    }
}
