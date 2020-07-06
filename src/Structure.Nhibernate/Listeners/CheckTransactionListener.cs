using NHibernate.Event;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Nhibernate.Listeners
{
    public class CheckTransactionListener : IPreInsertEventListener, IPreUpdateEventListener, IPreDeleteEventListener, ISaveOrUpdateEventListener, IMergeEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            EnsureTransaction(@event);
            return true;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            EnsureTransaction(@event);
            return true;
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            if (@event.Session.Transaction == null || !@event.Session.Transaction.IsActive)
            {
                throw new InvalidProgramException("Nenhuma transação foi iniciada.");
            }

            return true;
        }

        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {

        }

        public void OnMerge(MergeEvent @event, System.Collections.IDictionary copiedAlready)
        {

        }

        public void OnMerge(MergeEvent @event)
        {

        }

        public Task OnSaveOrUpdateAsync(SaveOrUpdateEvent @event, CancellationToken cancellationToken)
        {
            EnsureTransaction(@event);
            return Task.FromResult(true);
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            EnsureTransaction(@event);
            return Task.FromResult(true);
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            EnsureTransaction(@event);
            return Task.FromResult(true);
        }

        public Task OnMergeAsync(MergeEvent @event, CancellationToken cancellationToken)
        {
            EnsureTransaction(@event);
            return Task.FromResult(true);
        }

        public Task OnMergeAsync(MergeEvent @event, IDictionary copiedAlready, CancellationToken cancellationToken)
        {
            EnsureTransaction(@event);
            return Task.FromResult(true);
        }

        public Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
        {
            EnsureTransaction(@event);
            return Task.FromResult(true);
        }

        private static void EnsureTransaction(AbstractEvent @event)
        {
            if (@event.Session.Transaction == null || !@event.Session.Transaction.IsActive)
            {
                throw new InvalidProgramException("Nenhuma transação foi iniciada.");
            }
        }
    }
}
