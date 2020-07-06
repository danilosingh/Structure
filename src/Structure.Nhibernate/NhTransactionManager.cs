using NHibernate;
using Structure.Data;
using Structure.Data.Filtering;
using Structure.Extensions;
using Structure.Nhibernate.Filtering;
using System.Threading.Tasks;

namespace Structure.Nhibernate.Data
{
    public class NhTransactionManager : ITransactionManager
    {
        private readonly INhSessionProvider sessionAccessor;
        private readonly IDataFilterHandler dataFilterHandler;
        private ITransaction transaction;

        public bool TransactionActive
        {
            get { return transaction?.IsActive ?? false; }
        }

        public NhTransactionManager(INhSessionProvider sessionAccessor, IDataFilterHandler dataFilterHandler)
        {
            this.sessionAccessor = sessionAccessor;
            this.dataFilterHandler = dataFilterHandler;
        }

        public void BeginTransaction()
        {
            if (sessionAccessor.Session.Transaction != null &&
                sessionAccessor.Session.Transaction.IsActive)
            {
                throw new StructureException("A transação já foi aberta.");
            }

            //TODO: Criar parâmetro para desabilitar Session.Clear() 
            //Essa opção não pode ser removida no momento, devido o automapper setar valores em objetos persistentes no salvar
            sessionAccessor.Session.Clear();
            transaction = sessionAccessor.Session.BeginTransaction();
            ApplyGlobalFilters();
        }


        protected virtual void ApplyGlobalFilters()
        {
            foreach (var dataFilter in dataFilterHandler.GetFilters())
            {
                if (dataFilter.IsEnabled)
                {
                    EnableFilter(dataFilter.ToNhDataFilter());
                }
                else
                {
                    DisableFilter(dataFilter.Name);
                }
            }          
        }

        private void EnableFilter(NhDataFilter nhDataFilter)
        {
            var filter = sessionAccessor.Session.GetEnabledFilter(nhDataFilter.Name) ?? sessionAccessor.Session.EnableFilter(nhDataFilter.Name);

            foreach (var parameter in nhDataFilter.FilterDefinition.Parameters)
            {
                filter.SetParameter(parameter.Key.Remove(":"), parameter.Value.Value);
            }
        }

        private void DisableFilter(string name)
        {
            if (sessionAccessor.Session.GetEnabledFilter(name) != null)
            {
                sessionAccessor.Session.DisableFilter(name);
            }
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            transaction?.Dispose();
            sessionAccessor.Dispose();
        }

        public Task CommitAsync()
        {
            return transaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return transaction.RollbackAsync();
        }
    }
}
