using Structure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structure.ExceptionHandling
{
    [Serializable]
    public class ErrorInfo
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }
        
        public IList<ValidationError> ValidationErrors { get; set; }

        public ErrorInfo()
        { }

        public ErrorInfo(string message)
        {
            Message = message;
        }

        public ErrorInfo(string message, string details)
            : this(message)
        {
            Details = details;
        }

        public ErrorInfo(string code, string message, string details)
            : this(message, details)
        {
            Code = code;
        }

        public IList<ValidationError> GetValidationErrors()
        {
            return ValidationErrors != null  && ValidationErrors.Count > 0 ? ValidationErrors.ToList() : new List<ValidationError>() { new ValidationError(Message) };
        }
    }
}