using System.Threading.Tasks;

namespace Structure.Messaging.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
