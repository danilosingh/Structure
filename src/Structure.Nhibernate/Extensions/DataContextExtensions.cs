using NHibernate;
using Structure.Data;
using System;

namespace Structure.Nhibernate
{
    public class DataContextExtensions
    {
        public ISQLQuery CreateSQLQuery(IDataContext context, string queryString)
        {
            if (context is INhDataContext nhDataContext)
            {
                return nhDataContext.Session.CreateSQLQuery(queryString);
            }

            throw new NotSupportedException("Data context don't support");
        }
    }
}
