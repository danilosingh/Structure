using System;

namespace Structure.Security.Authorization
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
        { }

        public AuthorizationException(string message) : base(message)
        { }
    }
}
