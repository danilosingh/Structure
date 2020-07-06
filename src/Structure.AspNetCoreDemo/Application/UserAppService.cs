using Microsoft.AspNetCore.Identity;
using Structure.Application;
using Structure.AspNetCoreDemo.Events.Integration;
using Structure.Domain.Queries;
using Structure.Messaging.EventBus.Abstractions;
using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Dtos;
using Structure.Tests.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Application
{
    public interface IUserAppService : ICrudApplicationService<User, Guid, UserDto>
    {
        Task Register(string name);
    }

    public class UserAppService : CrudApplicationService<User, Guid, UserDto, IUserRepository>, IUserAppService
    {
        private readonly IDistributedEventStore eventStore;

        public UserAppService(IUserRepository repository, UserManager<User> userManager, IDistributedEventStore eventStore) : base(repository)
        {
            this.eventStore = eventStore;
        }

        protected override void PrepareQueryInput(FilterableQueryInput queryInput)
        {
            base.PrepareQueryInput(queryInput);
            queryInput.Fields = new string[] { "Name", "Email" };
        }

        public override Task<UserDto> CreateAsync(UserDto dto)
        {
            return base.CreateAsync(dto);
        }

        public async Task Register(string name)
        {
            var user = await repository.GetAsync(new Guid("8f9290ea-0d74-4ceb-8288-246b733b8240"));
            eventStore.Add(new UserRegistered() { UserId = user.Id });
            user.Register("Danilo Singh");
            await repository.UpdateAsync(user);
        }
    }


}
