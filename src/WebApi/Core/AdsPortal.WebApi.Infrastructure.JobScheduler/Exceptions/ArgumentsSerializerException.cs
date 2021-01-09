namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class ArgumentsSerializerException : Exception
    {
        public ArgumentsSerializerException()
        {

        }

        public ArgumentsSerializerException(string? message) : base(message)
        {

        }

        public ArgumentsSerializerException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        protected ArgumentsSerializerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
