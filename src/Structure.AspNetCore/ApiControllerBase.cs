using Microsoft.AspNetCore.Mvc;
using Structure.AspNetCore.Extensions;
using Structure.AspNetCore.Mvc.Model;
using Structure.Domain.Notifications;
using Structure.Security.Extensions;
using Structure.Session;
using System;

namespace Structure.AspNetCore
{
    public class ApiControllerBase : ControllerBase
    {
        public INotificationCollection Notifications
        {
            get { return ControllerContext.HttpContext.GetService<INotificationCollection>(); }
        }

        public IAppSession GetSession()
        {
            return ControllerContext.HttpContext.GetService<IAppSession>();
        }

        protected T GetClaim<T>(string claimType)
        {
            return User.GetClaimsIdentity().GetClaim<T>(claimType);
        }

        protected IActionResult Result<T>(T obj, bool returnNotFoundIfIsNull = true)
        {
            if (returnNotFoundIfIsNull && obj == null)
            {
                return NotFound();
            }

            return Ok(new ResultResponse<T>(obj));
        }

        protected IActionResult Result(bool success)
        {
            return Ok(new ResultResponse(success));
        }

        protected IActionResult Result<T, TResult>(T obj, Func<T, TResult> transform, bool returnNotFoundIfIsNull = true)
        {
            if (returnNotFoundIfIsNull && obj == null)
            {
                return NotFound();
            }

            return Ok(new ResultResponse<TResult>(transform(obj)));
        }

        protected virtual IActionResult BadRequest(INotificationCollection notifications)
        {
            return new NotificationActionResult(notifications);
        }

        protected virtual T GetService<T>()
            where T : class
        {
            return ControllerContext.HttpContext.GetService<T>();
        }
    }
}
