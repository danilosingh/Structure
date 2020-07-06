using Microsoft.Extensions.DependencyInjection;
using Structure.Application;
using Structure.Data;
using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Dtos;
using Structure.Tests.Shared.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.AspNetCoreDemo.Application
{
    public interface ITopicAppService : ICrudApplicationService<Topic, Guid, TopicDto>
    {
        void InvertOrdination();
    }

    public class TopicAppService : CrudApplicationService<Topic, Guid, TopicDto, ITopicRepository>, ITopicAppService
    {
        public TopicAppService(ITopicRepository repository) : base(repository)
        {
        }

        protected override async Task OnBeforeCreateAsync(TopicDto dto, Topic entity)
        {
             await base.OnBeforeCreateAsync(dto, entity);
        }

        public void InvertOrdination()
        {
            var topics = repository.GetAll(new Domain.Queries.FilterableQueryInput())
                .Items.OrderByDescending(c => c.Ordination)
                .ToList();

            for (int i = 0; i < topics.Count; i++)
            {
                topics[i].Ordination = i + 1;
            }

            ServiceProvider.GetService<IDataContext>().SaveChangesAsync().Wait();

            repository.UpdateMaterializedView();
        }
    }
}
