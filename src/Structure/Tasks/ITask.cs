using System;
using System.Threading.Tasks;

namespace Structure.Tarefas
{
    public interface ITask : IDisposable
    {
        bool Running { get; }

        void Stop();
        Task Start();
    }
}
