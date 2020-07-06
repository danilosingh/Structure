using Structure.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Structure.AspNetCore.Extensions
{
    public static class ActionExecutingContextExtensions
    {
        public static MethodInfo GetMethodInfo(this ActionExecutingContext context)
        {
            return context.ActionDescriptor.GetMethodInfo();
        }

        public static object[] GetParameterValues(this ActionExecutingContext context)
        {
            var method = context.GetMethodInfo();
            var parameters = method.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = context.ActionArguments.GetOrDefault(parameters[i].Name);
            }

            return parameterValues;
        }
    }
}
