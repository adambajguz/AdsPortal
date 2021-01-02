namespace MagicOperations.Extensions
{
    using System;
    using System.Runtime.Serialization;

    public class ArgumentBinderException : Exception
    {
        public ArgumentBinderException()
        {

        }

        public ArgumentBinderException(string? message) : base(message)
        {

        }

        public ArgumentBinderException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        protected ArgumentBinderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
