using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structure.Messaging.EventBus.Abstractions
{
    public interface IEventBusProcessingContextProvider
    {
        IEventBusProcessingContext CreateContext();
    }
}
