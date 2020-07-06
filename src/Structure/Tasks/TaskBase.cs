using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Tarefas
{
    public abstract class TaskBase : ITask
    {
        protected bool running;
        protected bool disposed;
        protected CancellationTokenSource cancellationToken;

        public bool Running
        {
            get { return running; }
        }

        public Task Start()
        {
            if (running)
            {
                throw new InvalidOperationException("Tarefa já iniciada");
            }

            if (disposed)
            {
                throw new ObjectDisposedException("Tarefa descartada");
            }

            running = true;
            cancellationToken = new CancellationTokenSource();
            return Task.Run(() => OnStart(cancellationToken), cancellationToken.Token);
        }

        public void Stop()
        {
            running = false;

            if (cancellationToken != null)
            {
                cancellationToken.Token.Register(OnStop, true);
                cancellationToken.Cancel();
            }
        }

        protected abstract void OnStart(CancellationTokenSource cancellationToken);

        protected abstract void OnStop();

        public virtual void Dispose()
        {
            this.disposed = true;
        }
    }
}
