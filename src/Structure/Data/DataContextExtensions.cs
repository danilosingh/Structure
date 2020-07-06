using System;
using System.Data;

namespace Structure.Data
{
    public static class DataContextExtensions
    {        
        public static IDbConnection Connection(this IDataContext dataContext)
        {
            if (dataContext is IAdoSupport adoContext)
            {
                return adoContext.DbConnection;
            }

            throw new NotSupportedException("Data context don't support ADO");
        }
    }
}
