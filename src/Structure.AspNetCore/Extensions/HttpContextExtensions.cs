using Microsoft.AspNetCore.Http;

namespace Structure.AspNetCore.Extensions
{
    public static class HttpContextExtensions
    {
        public static T GetService<T>(this HttpContext context)
        {
            return (T)context.RequestServices.GetService(typeof(T));
        }
    }
}
