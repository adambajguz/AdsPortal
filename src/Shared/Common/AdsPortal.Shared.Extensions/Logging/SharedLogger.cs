namespace AdsPortal.Shared.Extensions.Logging
{
    using System;
    using Microsoft.Extensions.Logging;

    public static class SharedLogger
    {
        public static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();
        public static ILogger CreateLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }

        public static ILogger CreateLogger(Type type)
        {
            return LoggerFactory.CreateLogger(type);
        }

        public static ILogger CreateLogger(string categoryName)
        {
            return LoggerFactory.CreateLogger(categoryName);
        }
    }
}
