using System.Threading.Tasks;

namespace Structure.Application
{
    internal class InnerUnitOfWork : IUnitOfWork
    {
        public UnitOfWorkStatus Status { get; private set; }

        public void Begin(UnitOfWorkOptions options)
        {
            Status = UnitOfWorkStatus.InProgress;
        }

        public InnerUnitOfWork()
        {
            Status = UnitOfWorkStatus.Created;
        }

        public Task CompleteAsync()
        {
            Status = UnitOfWorkStatus.Completed;
            return Task.FromResult(true);
        }

        public void Dispose()
        {
            if (Status == UnitOfWorkStatus.Completed)
            {
                return;
            }

            Status = UnitOfWorkStatus.Failed;
        }

        public Task SaveChangesAsync()
        {
            return Task.FromResult(true);
        }
    }
}