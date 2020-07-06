using Structure.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.Validation
{
    public class ValidationException : Exception
    {
        private readonly List<ValidationError> errors = new List<ValidationError>();

        public IEnumerable<ValidationError> Errors
        {
            get { return errors; }
        }

        public ValidationException(string errorMessage)
            : base(errorMessage)
        {
            errors.Add(new ValidationError(errorMessage));
        }

        public ValidationException(INotification notification)
            : base(notification.Message)
        {
            errors.Add(new ValidationError(notification.Message, notification.Path));
        }

        public ValidationException(IEnumerable<INotification> notifications)
        {
            errors.AddRange(notifications.Select(c => new ValidationError(c.Message, c.Path)));
        }

        public ValidationException(IEnumerable<ValidationError> errors)
        {
            this.errors.AddRange(errors);
        }
    }
}
