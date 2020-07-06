using Microsoft.AspNetCore.Mvc.Filters;
using Structure.AspNetCore.Extensions;

namespace Structure.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IActionExceptionHandler resultHandler;

        public ExceptionFilter(IActionExceptionHandler resultHandler)
        {
            this.resultHandler = resultHandler;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            HandleException(context);
        }

        protected virtual void HandleException(ExceptionContext context)
        {            
            context.Result = resultHandler.HandleException(context, context.Exception);
            context.Exception = null; //Handled!
        }
    }
}
