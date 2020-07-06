using System;
using System.Threading.Tasks;

namespace Structure.Data
{
    public interface ITransactionManager : IDisposable
    {
        bool TransactionActive { get; }

        void BeginTransaction();

        void Commit();
        Task CommitAsync();

        void Rollback();
        Task RollbackAsync();
    }
}
