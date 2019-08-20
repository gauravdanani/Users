using System;

namespace Users.Application.Exceptions
{
    public class InvalidJsonFormatException: FormatException
    {
        public InvalidJsonFormatException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
