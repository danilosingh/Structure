using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Structure.Messaging.EventBus.Abstractions
{
    public interface IEventBusProcessingContext : IDisposable
    {
        IServiceProvider ServiceProvider { get; }
        Task CompleteAsync();        
    }
}