using NHibernate;

namespace Structure.Nhibernate.Data
{
    public class NhSessionProvider : INhSessionProvider
    {
        private ISession session;
        private IStatelessSession statelessSession;
        private bool disposed = false;

        private readonly ISessionFactory sessionFactory;

        public ISession Session
        {
            get
            {
                if (session == null)
                {
                    session = sessionFactory.OpenSession();
                }

                return session;
            }
        }

        public IStatelessSession StatelessSession
        {
            get { return (statelessSession = sessionFactory.OpenStatelessSession()); }
        }


        public NhSessionProvider(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                session?.Dispose();
                statelessSession?.Dispose();
            }

            disposed = true;
        }
    }
}
