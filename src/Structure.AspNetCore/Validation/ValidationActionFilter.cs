using Microsoft.AspNetCore.Mvc.Filters;
using Structure.AspNetCore.Extensions;
using Structure.Validation;
using Structure.Validation.Interception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.AspNetCore.Validation
{
    public class ValidationActionFilter : IAsyncActionFilter
    {
        private readonly Lazy<IMethodInvocationValidator> validator;

        public ValidationActionFilter(Lazy<IMethodInvocationValidator> validator)
        {
            this.validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            var errors = validator.Value.Validate(context.GetMethodInfo(), context.GetParameterValues()) ?? new List<ValidationError>();
            AddModelStateErrors(context, errors);

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            else
            {
                await next();
            }
        }

        private void AddModelStateErrors(ActionExecutingContext context, IList<ValidationError> erros)
        {
            foreach (var state in context.ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    erros.Add(new ValidationError(error.ErrorMessage, state.Key));
                }
            }
        }
    }
}
