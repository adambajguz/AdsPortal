namespace AdsPortal.WebAPI.Exceptions.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    public class ExceptionResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public object Errors { get; }

        public ExceptionResponse(HttpStatusCode statusCode, string message, object errors)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        public override bool Equals(object? obj)
        {
            return obj is ExceptionResponse other &&
                   StatusCode == other.StatusCode &&
                   Message == other.Message &&
                   EqualityComparer<object>.Default.Equals(Errors, other.Errors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StatusCode, Message, Errors);
        }
    }
}
