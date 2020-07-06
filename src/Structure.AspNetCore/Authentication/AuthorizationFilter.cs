using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Structure.AspNetCore.Authentication;
using Structure.Security.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;
using IAuthorizationService = Structure.Security.Authorization.IAuthorizationService;

namespace Structure.AspNetCore.Mvc.ExceptionHandling
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IActionExceptionHandler actionResultFactory;
        private readonly IOptions<AspNetAuthorizationOptions> options;
        
        public AuthorizationFilter(IAuthorizationService authorizationService,
            IActionExceptionHandler actionResultFactory, 
            IOptions<AspNetAuthorizationOptions> options)
        {
            this.authorizationService = authorizationService;
            this.actionResultFactory = actionResultFactory;
            this.options = options;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                await authorizationService.AuthorizeAsync(context, options.Value, new CancellationToken());
            }
            catch (Exception ex)
            {
                context.Result = actionResultFactory.HandleException(context, ex);
            }            
        }
    }
}