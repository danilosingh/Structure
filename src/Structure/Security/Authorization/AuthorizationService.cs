using Structure.Session;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structure.Security.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAppSession appSession;
        private readonly IGrantChecker permissionChecker;

        public AuthorizationService(IAppSession appSession, IGrantChecker permissionChecker)
        {
            this.appSession = appSession;
            this.permissionChecker = permissionChecker;
        }

        public async Task AuthorizeAsync(IEnumerable<IAuthorizeInfo> authorizeInfos, CancellationToken cancellationToken)
        {
            if (appSession.UserId == null)
            {
                throw new AuthorizationException("Usuário não autenticado.");
            }

            foreach (var item in authorizeInfos)
            {
                await CheckGrantsAsync(item, cancellationToken);
            }
        }

        protected virtual async Task CheckGrantsAsync(IAuthorizeInfo authorizeInfo, CancellationToken cancellationToken)
        {
            if (await permissionChecker.IsGrantedAsync(authorizeInfo.RequireAll, authorizeInfo.Permissions, cancellationToken))
            {
                return;
            }

            throw new AuthorizationException("Permissões necessárias não estão concedidas.");
        }
    }
}
