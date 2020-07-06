using Microsoft.AspNetCore.Mvc;
using System;

namespace Structure.AspNetCore.Mvc.ExceptionHandling
{
    public interface IActionExceptionHandler
    {
        IActionResult HandleException(ActionContext context, Exception exception);
    }
}
