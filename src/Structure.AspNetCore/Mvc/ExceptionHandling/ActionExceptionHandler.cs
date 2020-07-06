using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Structure.AspNetCore.Extensions;
using Structure.AspNetCore.Helpers;
using Structure.ExceptionHandling;
using Structure.Security.Authorization;
using Structure.Validation;
using System;
using System.Collections.Generic;
using System.Net;

namespace Structure.AspNetCore.Mvc.ExceptionHandling
{
    public class ActionExceptionHandler : IActionExceptionHandler
    {
        private readonly IErrorInfoBuilder errorInfoBuilder;
        private readonly ILogger<ActionExceptionHandler> logger;

        public ActionExceptionHandler(IErrorInfoBuilder errorInfoBuilder, ILogger<ActionExceptionHandler> logger)
        {
            this.errorInfoBuilder = errorInfoBuilder;
            this.logger = logger;
        }

        public IActionResult HandleException(ActionContext context, Exception exception)
        {
            try
            {
                var errorInfo = errorInfoBuilder.BuildInfo(exception);

                if (ActionResultHelper.IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
                {
                    return exception switch
                    {
                        AuthorizationException _ => CreateFromAuthorizationException(context, errorInfo),
                        ValidationException _ => CreateFromValidationException(errorInfo),
                        _ => CreateFromGenericException(errorInfo),
                    };
                }
                else
                {
                    return new ChallengeResult();
                }
            }
            finally
            {
                logger.LogError(exception, "Error on action {0}", context.ActionDescriptor?.DisplayName);
            }
        }

        private IActionResult CreateFromAuthorizationException(ActionContext context, ErrorInfo errorInfo)
        {
            return CreateResult(errorInfo.GetValidationErrors(),
                context.HttpContext.User.Identity.IsAuthenticated ?
                    HttpStatusCode.Forbidden :
                    HttpStatusCode.Unauthorized);
        }

        private IActionResult CreateFromValidationException(ErrorInfo errorInfo)
        {
            return CreateResult(errorInfo.ValidationErrors, HttpStatusCode.BadRequest);
        }

        private IActionResult CreateFromGenericException(ErrorInfo errorInfo)
        {
            return new ObjectResult(errorInfo)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        private IActionResult CreateResult(IList<ValidationError> errors, HttpStatusCode statusCode)
        {
            return new ObjectResult(new { errors })
            {
                StatusCode = (int)statusCode
            };
        }
    }
}
