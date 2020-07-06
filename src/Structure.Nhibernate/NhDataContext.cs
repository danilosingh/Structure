using NHibernate;
using NHibernate.Transaction;
using Structure.Auditing;
using Structure.Domain.Entities;
using Structure.Domain.Events;
using Structure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Nhibernate
{
    //TODO: Create generic DataContext to implement ApplyGlobalFilters/Dispatch and other generic methods (
    public class NhDataContext : INhDataContext
    {
        protected readonly INhSessionProvider sessionProvider;
        private readonly IEntityPropertySetter propertySetter;
        private readonly IServiceProvider serviceProvider;
        private IList<IEntity> trackedEntities;

        public ISession Session
        {
            get { return sessionProvider.Session; }
        }

        public IDbConnection DbConnection
        {
            get { return Session?.Connection; }
        }

        public IDbTransaction CurrentTransaction
        {
            get
            {
                return NhibernateReflectionMethods.AdoTransactionField.GetValue(Session.Transaction) as IDbTransaction;
            }
        }

        public NhDataContext(INhSessionProvider sessionProvider,
            IEntityPropertySetter propertySetter, IServiceProvider serviceProvider)
        {
            this.sessionProvider = sessionProvider;
            this.propertySetter = propertySetter;
            this.serviceProvider = serviceProvider;
            trackedEntities = new List<IEntity>();
        }

        public void Dispose()
        {
            sessionProvider.Dispose();
        }

        public T Get<T, TId>(TId id)
        {
            return Session.Get<T>(id);
        }

        public async Task<T> GetAsync<T, TId>(TId id)
        {
            return await Session.GetAsync<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }

        private void TrackEntity<T>(T entity)
        {
            if (entity is IEntity convertedEntity)
            {
                trackedEntities.Add(convertedEntity);
            }
        }

        public void Create<T>(T entity)
        {
            propertySetter.SetCreationProperties(entity);
            TrackEntity(entity);
            Session.Save(entity);
        }

        public async Task CreateAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            propertySetter.SetCreationProperties(entity);
            TrackEntity(entity);
            await Session.SaveAsync(entity);
        }

        public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            propertySetter.SetModificationProperties(entity);
            TrackEntity(entity);
            await Session.UpdateAsync(entity, cancellationToken);
        }

        public void Update<T>(T entity)
        {
            propertySetter.SetModificationProperties(entity);
            TrackEntity(entity);
            Session.Update(entity);
        }

        public void Delete<T>(T entity)
        {
            if (entity is ISoftDelete entitySoftDelete)
            {
                entitySoftDelete.IsDeleted = true;
                Update(entitySoftDelete);
            }
            else
            {
                Session.Delete(entity);
            }
        }

        public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is ISoftDelete entitySoftDelete)
            {
                entitySoftDelete.IsDeleted = true;
                await UpdateAsync(entitySoftDelete);
            }
            else
            {
                await Session.DeleteAsync(entity, cancellationToken);
            }
        }

        public async Task SaveChangesAsync()
        {
            await Session.FlushAsync();
            await DispatchDomainEventsAsync();
        }

        private async Task DispatchDomainEventsAsync()
        {
            foreach (var aggregateRoot in trackedEntities.OfType<IAggregateRoot>())
            {
                foreach (var @event in aggregateRoot.DomainEvents)
                {
                    var eventType = @event.GetType();
                    var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
                    var handler = serviceProvider.GetService(handlerType);
                    await Task.Yield();
                    await (Task)handlerType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
        }

        public async Task MergeAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is IEntity entityAux)
            {
                if (!entityAux.HasIdentifier())
                {
                    propertySetter.SetCreationProperties(entityAux);
                }
                else
                {
                    propertySetter.SetModificationProperties(entityAux);
                }
            }

            await Session.MergeAsync(entity, cancellationToken);
        }
    }

    public static class NhibernateReflectionMethods
    {
        public static FieldInfo AdoTransactionField = TypeHelper.GetField(typeof(AdoTransaction), "trans");
    }
}
