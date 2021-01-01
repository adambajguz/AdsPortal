namespace MagicOperations.Extensions
{
    using System;
    using System.Runtime.Serialization;

    public class ApiException : Exception
    {
        public ApiException()
        {

        }

        public ApiException(string? message) : base(message)
        {

        }

        public ApiException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
