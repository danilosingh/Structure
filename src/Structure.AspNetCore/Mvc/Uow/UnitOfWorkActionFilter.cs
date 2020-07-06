using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Structure.Application;
using Structure.AspNetCore.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.AspNetCore.Mvc.Uow
{
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly UnitOfWorkOptions options;

        public UnitOfWorkActionFilter(IUnitOfWorkManager unitOfWorkManager, IOptions<UnitOfWorkOptions> options)
        {
            this.unitOfWorkManager = unitOfWorkManager;
            this.options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionDescriptor.IsControllerAction())
            {
                await next();
                return;
            }

            var methodInfo = context.ActionDescriptor.GetMethodInfo();
            var attr = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().FirstOrDefault();

            if (attr != null && !attr.Enabled)
            {
                await next();
                return;
            }

            using (var uow = unitOfWorkManager.Begin(options))
            {
                var result = await next();

                if (result.Exception == null || result.ExceptionHandled)
                {
                    await uow.CompleteAsync();
                }
            }
        }
    }
}
