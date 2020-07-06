using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Structure.AspNetCore.Helpers
{
    public static class ActionResultHelper
    {
        public static bool IsObjectResult(Type returnType)
        {
            if (returnType == typeof(Task))
            {
                returnType = typeof(void);
            }
            else if (returnType.GetTypeInfo().IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = returnType.GenericTypeArguments[0];
            }

            if (typeof(IActionResult).GetTypeInfo().IsAssignableFrom(returnType))
            {
                return true;
            }

            return true;
        }
    }
}
