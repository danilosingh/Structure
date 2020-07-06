using System.Data;

namespace Structure.Data
{
    public interface IAdoSupport
    {
        IDbConnection DbConnection { get; }
        IDbTransaction CurrentTransaction { get; }
    }
}
