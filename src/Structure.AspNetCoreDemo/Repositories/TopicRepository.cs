using Dapper;
using Structure.Collections;
using Structure.Data;
using Structure.Data.Repositories;
using Structure.Domain.Queries;
using Structure.Tests.Shared.Domain.Repositories;
using Structure.Tests.Shared.Entities;
using System;
using System.Linq;

namespace Structure.AspNetCoreDemo.Repositories
{
    public class TopicRepository : Repository<Topic, Guid>, ITopicRepository
    {
        public TopicRepository(IDataContext context) : base(context)
        {
        }

        public override IPagedList<Topic> GetAll(FilterableQueryInput parameters)
        {
            return base.GetAll(parameters);
        }

        public void UpdateMaterializedView()
        {
            context.Connection().Execute("REFRESH MATERIALIZED VIEW vw_topic_tree_full_ordination");
        }

        protected override IPagedList<Topic> ExecuteQuery(IQueryable<Topic> query, FilterableQueryInput parameters, int totalCount)
        {

            return base.ExecuteQuery(query, parameters, totalCount);
        }

    }
}
