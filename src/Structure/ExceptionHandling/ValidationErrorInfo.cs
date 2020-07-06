using System;

namespace Structure.ExceptionHandling
{
    [Serializable]
    public class ValidationErrorInfo
    {
        public string Message { get; set; }
        public string Member { get; set; }

        public ValidationErrorInfo()
        {
        }

        public ValidationErrorInfo(string message)
        {
            Message = message;
        }

        public ValidationErrorInfo(string message, string member)
            : this(message)
        {
            Member = member;
        }
    }
}