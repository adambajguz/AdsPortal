namespace MagicOperations.Extensions
{
    using System;
    using System.Runtime.Serialization;

    public class MagicOperationsException : Exception
    {
        public MagicOperationsException()
        {

        }

        public MagicOperationsException(string? message) : base(message)
        {

        }

        public MagicOperationsException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        protected MagicOperationsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
