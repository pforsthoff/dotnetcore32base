using System;

namespace Cheetas3.EU.Application.Common.Exceptions
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException()
            : base()
        {
        }

        public InvalidTypeException(string name)
            : base($"Type \"{name}\" is invalid.")
        {
        }

        public InvalidTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
