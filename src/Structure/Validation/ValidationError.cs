using System;

namespace Structure.Validation
{
    [Serializable]
    public class ValidationError
    {
        public string Message { get; set; }
        public string Member { get; set; }

        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationError(string message, string member) : this(message)
        {
            Member = member;
        }
    }
}
