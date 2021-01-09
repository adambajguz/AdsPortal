namespace AdsPortal.WebApi.Application.Exceptions
{
    using System;

    public class ValidationFailedException : Exception
    {
        public string? PropertyName { get; }

        public ValidationFailedException()
        {

        }

        public ValidationFailedException(string message) : base(message)
        {

        }

        public ValidationFailedException(string? propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        public ValidationFailedException(string? propertyName, string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = propertyName;
        }

        public ValidationFailedException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
