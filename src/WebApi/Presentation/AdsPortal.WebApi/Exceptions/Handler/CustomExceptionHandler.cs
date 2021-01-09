namespace AdsPortal.WebApi.Exceptions.Handler
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.WebApi.Application.Exceptions;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public static class CustomExceptionHandler
    {
        public static async Task HandleExceptionAsync(HttpContext context)
        {
            // Try and retrieve the error from the ExceptionHandler middleware
            IExceptionHandlerFeature? exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            Exception? ex = exceptionDetails?.Error;

            await HandleExceptionAsync(context, ex);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception? exception)
        {
            object errorObject = new object();
            HttpStatusCode code = exception switch
            {
                FluentValidation.ValidationException ex => HandleValidationException(ex, ref errorObject),
                ValidationFailedException ex => HandleValidationFaliedException(ex, ref errorObject),
                ForbiddenException _ => HttpStatusCode.Forbidden,
                NotFoundException _ => HttpStatusCode.NotFound,
                _ => HandleUnknownException(context, exception)
            };

            string message = exception?.Message ?? "Unknown exception";

            ExceptionResponse response = new ExceptionResponse(code, message, errorObject);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;

            string responseJson = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(responseJson);
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
            loggerFactory.CreateLogger(typeof(CustomExceptionHandler).FullName).LogError(exception, "Unhandled exception.");

            return HttpStatusCode.InternalServerError;
        }
    }
}