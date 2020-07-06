using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Security.Authorization
{
    public interface IAuthorizationService
    {           
        Task AuthorizeAsync(IEnumerable<IAuthorizeInfo> authorizeData, CancellationToken cancellationToken);
    }
}
