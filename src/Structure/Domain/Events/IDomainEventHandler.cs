using System.Threading.Tasks;

namespace Structure.Domain.Events
{
    public interface IDomainEventHandler<T> 
        where T : IDomainEvent
    {
        Task Handle(T @event);
    }
}
