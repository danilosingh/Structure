using System;

namespace Structure
{
    public class StructureException : Exception
    {
        public StructureException()
        {
        }

        public StructureException(string message) : base(message)
        {
        }

        public StructureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
