using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Structure.AspNetCore.Authorization;
using Structure.AspNetCore.Extensions;
using Structure.Extensions;
using Structure.Helpers;
using Structure.Security.Authorization;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using IAuthorizationService = Structure.Security.Authorization.IAuthorizationService;

namespace Structure.AspNetCore.Authentication
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task AuthorizeAsync(this IAuthorizationService authorizationService, AuthorizationFilterContext context, AspNetAuthorizationOptions options, CancellationToken cancellationToken)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor))
            {
                return;
            }

            var authorizeInfo = GetAuthorizeInfo(actionDescriptor, options);

            if (authorizeInfo != null)
            {
                await authorizationService.AuthorizeAsync(new IAuthorizeInfo[] { authorizeInfo }, cancellationToken);
            }
        }

        private static IAuthorizeInfo GetAuthorizeInfo(ControllerActionDescriptor actionDescriptor, AspNetAuthorizationOptions options)
        {
            var methodInfo = actionDescriptor.GetMethodInfo();

            if (!methodInfo.IsPublic && !methodInfo.GetCustomAttributes().OfType<IAuthorizeInfo>().Any())
            {
                return null;
            }

            if (AllowAnonymous(methodInfo, methodInfo.DeclaringType))
            {
                return null;
            }

            var authorizeInfo = TypeHelper
                .GetAttributesOfMember<IAuthorizeInfo>(actionDescriptor.MethodInfo)
                .FirstOrDefault();

            if (authorizeInfo == null)
            {
                authorizeInfo = TypeHelper
                    .GetAttributesOfType<IAuthorizeInfo>(actionDescriptor.ControllerTypeInfo.AsType())
                    .FirstOrDefault();

                if (authorizeInfo != null && authorizeInfo.Permissions.Length == 0 && options.UseConventionedPermissions)
                {
                    var area = !authorizeInfo.Area.IsNullOrWhiteSpace() ? authorizeInfo.Area + "." : "";
                    authorizeInfo = new ProtectedAttribute($"{area}{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}");
                }
            }

            return authorizeInfo;
        }

        private static bool AllowAnonymous(MethodInfo methodInfo, Type declaringType)
        {
            return TypeHelper.GetAttributesOfMemberAndType(methodInfo, declaringType)
                .OfType<IAllowAnonymous>()
                .Any();
        }
    }
}
