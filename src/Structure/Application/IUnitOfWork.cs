using System;
using System.Threading.Tasks;

namespace Structure.Application
{
    public interface IUnitOfWork : IDisposable
    {
        UnitOfWorkStatus Status { get; }
        
        Task SaveChangesAsync();
        Task CompleteAsync();
        void Begin(UnitOfWorkOptions options);
    }

    public delegate void UnitOfWorkEventHandler(IUnitOfWork unitOfWork);
}
