﻿namespace AdsPortal.Analytics
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AnalyticsRecordOperations.Commands.CreateOrUpdateAnalyticsRecord;
    using MediatR;
    using MediatR.GenericOperations.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;

    //TODO: add min max average request to respone time
    public class AnalyticsMiddleware
    {
        private readonly RequestDelegate _next;

        public AnalyticsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ILogger logger = context.RequestServices.GetRequiredService<ILogger<AnalyticsMiddleware>>();

            try
            {
                HttpRequest request = context.Request;

                PathString path = context.Request.Path;

                if (!(path.Value?.Contains("debug/ws-proxy") ?? false))
                {
                    IPAddress ip = context.Connection.RemoteIpAddress ?? IPAddress.None;
                    StringValues userAgent = request.Headers[HeaderNames.UserAgent];

                    string sanitizedPath = path.Sanitize();

                    await CreateOrUpdateAnalyticsRecord(logger, context, ip, userAgent, sanitizedPath);

                    logger.LogDebug("Request to {Path} from {IP} and {UserAgent}", sanitizedPath, ip, userAgent);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in analytics middleware", ex);
            }

            await _next(context);
        }

        private static async Task CreateOrUpdateAnalyticsRecord(ILogger logger, HttpContext context, IPAddress ip, StringValues userAgent, string sanitizedPath)
        {
            IMediator? mediator = context.RequestServices.GetService<IMediator>();
            if (!(mediator is null))
            {
                CreateOrUpdateAnalyticsRecordCommand command = new CreateOrUpdateAnalyticsRecordCommand
                {
                    Uri = sanitizedPath,
                    UserAgent = userAgent.ToString(),
                    Ip = ip.ToString()
                };

                IdResult id = await mediator.Send(command);
                logger.LogDebug("Created or updated analytics record with id {Id}", id.Id);
            }
        }
    }

    public static class AnalyticsMiddlewareExtensions
    {
        public static IApplicationBuilder UseAnalytics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AnalyticsMiddleware>();
        }
    }
}
