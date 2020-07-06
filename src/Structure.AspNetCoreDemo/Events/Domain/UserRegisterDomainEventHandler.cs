using Structure.Domain.Events;
using Structure.Tests.Shared.Entities;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Events
{
    public class UserRegisterDomainEventHandler : IDomainEventHandler<UserRegisterDomainEvent>
    {
        public async Task Handle(UserRegisterDomainEvent @event)
        {
            await Task.CompletedTask;
        }
    }
}
