using Structure.Helpers;
using Structure.Security.Authorization;
using Structure.Validation;
using System;
using System.Linq;

namespace Structure.ExceptionHandling
{
    public class ErrorInfoBuilder : IErrorInfoBuilder
    {
        public ErrorInfo BuildInfo(Exception exception)
        {
            var errorInfo = CreateErrorInfo(exception);

            if (exception is IHasErrorCode exceptionWithErrorCode)
            {
                errorInfo.Code = exceptionWithErrorCode.Code;
            }

            return errorInfo;
        }

        private ErrorInfo CreateErrorInfo(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;

                if (aggException.InnerException is ValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (exception is ValidationException validationException)
            {
                return new ErrorInfo("ValidationError") { ValidationErrors = validationException.Errors.ToList() };
            }

            if (exception is AuthorizationException authorizationException)
            {
                return new ErrorInfo(authorizationException.Message);
            }

            //To refactoring
            var message = exception.Message;
            var stackTrace = exception.StackTrace;

            while (exception.InnerException != null)
            {
                message += $"{StringHelper.LineBreak(1)}{StringHelper.Repeat("-", 50)}{StringHelper.LineBreak(1)}{exception.InnerException.Message}";
                stackTrace += $"{StringHelper.LineBreak(1)}{StringHelper.Repeat("-", 50)}{StringHelper.LineBreak(1)}{exception.InnerException.StackTrace}";
                exception = exception.InnerException;
            }

            return new ErrorInfo(message, stackTrace);
        }
    }
}


