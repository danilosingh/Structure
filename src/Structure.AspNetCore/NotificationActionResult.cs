using Structure.Domain.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.AspNetCore
{
    public class NotificationActionResult : IActionResult
    {
        private readonly INotificationCollection notifications;

        public NotificationActionResult(INotificationCollection notifications)
        {
            this.notifications = notifications;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var result = new ObjectResult(new { validationErrors = notifications.ToList().Select(c => new { message = c.Message }) }) 
            { 
                StatusCode = StatusCodes.Status400BadRequest 
            };
            return result.ExecuteResultAsync(context);
        }
    }
}
