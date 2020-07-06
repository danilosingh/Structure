using System;

namespace Structure.Application
{
    public interface IUnitOfWorkManager : IDisposable
    {
        IUnitOfWork Begin();
        IUnitOfWork Begin(UnitOfWorkOptions options);
        bool HasActiveUnitOfWork();
    }
}
