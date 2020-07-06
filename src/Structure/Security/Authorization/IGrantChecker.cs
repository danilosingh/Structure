using System.Threading;
using System.Threading.Tasks;

namespace Structure.Security.Authorization
{
    public interface IGrantChecker
    {
        Task<bool> IsGrantedAsync(string permissionName, CancellationToken cancellationToken);
    }
}
