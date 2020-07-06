using NHibernate;
using Structure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Nhibernate
{
    public interface INhDataContext : IDataContext, IAdoSupport
    {
        ISession Session { get; }
        Task MergeAsync<T>(T entity, CancellationToken cancellationToken = default);
    }
}