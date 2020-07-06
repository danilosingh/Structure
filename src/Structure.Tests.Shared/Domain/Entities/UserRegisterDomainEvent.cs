using Structure.Domain.Events;
using Structure.Tests.Shared.Entities;

namespace Structure.Tests.Shared.Entities
{
    public class UserRegisterDomainEvent : IDomainEvent
    {
        public User User { get; set; }
    }
}
