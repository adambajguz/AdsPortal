﻿namespace AdsPortal.WebPortal
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using AdsPortal.Shared.Extensions.Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public static class Configuration
    {
        //private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// Adds "appsettings.json" and "appsettings.{environmentName}.json". This files should contain core configuration, including logging configuration.
        /// </summary>
        public static IConfigurationBuilder AddCoreConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext)
        {
            //string environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment) ?? "Production";
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

            //https://github.com/aspnet/MetaPackages/blob/master/src/Microsoft.AspNetCore/WebHost.cs#L198
            IConfigurationSource? chainedConfigurationProvider = builder.Sources.FirstOrDefault();

            if (!(chainedConfigurationProvider is ChainedConfigurationSource))
            {
                throw new ApplicationException($"First configuration source is not {nameof(ChainedConfigurationSource)}");
            }

            builder.Sources.Clear();
            builder.Add(chainedConfigurationProvider);

            builder.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
                   .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);

            return builder;
        }

        /// <summary>
        /// Adds other configuration providers that may throw exception during addition.
        /// </summary>
        public static IConfigurationBuilder AddConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext)
        {
            string contentDirectory = hostingContext.HostingEnvironment.ContentRootPath;
            string currentDirectory = Directory.GetCurrentDirectory();
            string diffDirectory = currentDirectory.TrimStart(contentDirectory);

            bool isDiffDirectoryEmpty = string.IsNullOrWhiteSpace(diffDirectory);
            if (!isDiffDirectoryEmpty && !diffDirectory.EndsWith(Path.DirectorySeparatorChar))
            {
                diffDirectory += Path.DirectorySeparatorChar;
            }
            else if (isDiffDirectoryEmpty)
            {
                diffDirectory = string.Empty;
            }

            //builder.AddApplicationConfigs(hostingContext, diffDirectory);

            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                var appAssembly = Assembly.Load(new AssemblyName(hostingContext.HostingEnvironment.ApplicationName));
                if (appAssembly != null)
                {
                    builder.AddUserSecrets(appAssembly, optional: true);
                }
            }

            builder.AddEnvironmentVariables();

            return builder;
        }
    }
}
