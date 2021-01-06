namespace MagicModels.Extensions
{
    using System;
    using System.Runtime.Serialization;

    public class MagicModelsException : Exception
    {
        public MagicModelsException()
        {

        }

        public MagicModelsException(string? message) : base(message)
        {

        }

        public MagicModelsException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        protected MagicModelsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
