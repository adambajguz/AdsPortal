namespace AdsPortal.WebApi.Persistence.AOP
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Castle.DynamicProxy;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;

    public class LoggingInterceptor : IInterceptor
    {
        private static ulong CallId { get; set; }

        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_logger.IsEnabled(LogLevel.Trace) && !invocation.TargetType.Name.Contains("Job") && !invocation.Method.Name.Contains("get_Job"))
            {
                Stopwatch stopwatch = new();

                _logger.LogTrace("#{CallId} Calling method {Type}.{Method}().", ++CallId, invocation.TargetType, invocation.Method.Name);

                stopwatch.Start();
                invocation.Proceed();
                stopwatch.Stop();

                _logger.LogTrace("#{CallId} Executed method {Type}.{Method}() returned {Return} after {Miliseconds}ms ({Ticks} ticks).", CallId, invocation.TargetType, invocation.Method.Name, invocation.ReturnValue, stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks);
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
