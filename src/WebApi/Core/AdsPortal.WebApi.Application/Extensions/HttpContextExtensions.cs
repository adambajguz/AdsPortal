namespace AdsPortal.WebApi.Application.Extensions
{
    using System;
    using Microsoft.AspNetCore.Http;

    public static class HttpContextExtensions
    {
        public static Uri GetAbsoluteUri(this HttpContext context)
        {
            HttpRequest request = context.Request;
            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }
    }
}
