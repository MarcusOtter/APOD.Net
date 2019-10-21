using System;

namespace Apod
{
    public class DateOutOfRangeException : ArgumentOutOfRangeException
    {
        public DateOutOfRangeException() { }
        public DateOutOfRangeException(string message) : base(string.Empty, message) { }
        public DateOutOfRangeException(string message, Exception inner) : base(message, inner) { }
        public DateOutOfRangeException(string parameterName, string message) : base(parameterName, message) { }
        public DateOutOfRangeException(string parameterName, DateTime dateTime) 
            : base(parameterName, $"The parameter \"{parameterName}\" must to be between 1995-06-16 and today's date. (Date provided: {dateTime.ToString("yyyy-MM-dd")})") { }
    }
}
