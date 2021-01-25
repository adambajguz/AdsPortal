namespace AdsPortal.WebApi.Exceptions.Handler
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Exceptions;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class CustomExceptionHandler
    {
        public static async Task HandleExceptionAsync(HttpContext context)
        {
            // Try and retrieve the error from the ExceptionHandler middleware
            IExceptionHandlerFeature? exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            Exception? ex = exceptionDetails?.Error;

            await HandleExceptionAsync(context, ex);
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception? exception)
        {
            object errorObject = new object();
            HttpStatusCode code = exception switch
            {
                FluentValidation.ValidationException ex => HandleValidationException(ex, ref errorObject),
                ValidationFailedException ex => HandleValidationFaliedException(ex, ref errorObject),
                ForbiddenException _ => HttpStatusCode.Forbidden,
                NotFoundException _ => HttpStatusCode.NotFound, //TODO: replace 404 with other code, e.g. bad request
                _ => HandleUnknownException(context, exception)
            };

            string message = exception?.Message ?? "Unknown exception";

            ExceptionResponse response = new ExceptionResponse(code, message, errorObject);

            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsJsonAsync(response);
            await context.Response.CompleteAsync();
        }

        private static HttpStatusCode HandleValidationException(FluentValidation.ValidationException ex, ref object errors)
        {
            errors = ex.Errors.Select(e => new ErrorModel(e.PropertyName, e.ErrorMessage))
                              .ToList();

            return HttpStatusCode.BadRequest;
        }

        private static HttpStatusCode HandleValidationFaliedException(ValidationFailedException ex, ref object errors)
        {
            errors = new ErrorModel(ex.PropertyName, ex.Message);

            return HttpStatusCode.BadRequest;
        }

        private static HttpStatusCode HandleUnknownException(HttpContext context, Exception? exception)
        {
            ILoggerFactory loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateLogger(typeof(CustomExceptionHandler).FullName);

            if (exception is null)
            {
                logger.LogError("Unhandled exception.");
            }
            else
            {
                logger.LogError(exception, "Unhandled exception.");
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}