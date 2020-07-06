using Microsoft.AspNetCore.Mvc.Filters;

namespace Structure.AspNetCore.Extensions
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                //TODO: hangle exception
                // context.ExceptionHandled = true;
            }
        }
    }
}
