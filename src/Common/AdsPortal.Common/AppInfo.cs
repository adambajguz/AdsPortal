namespace AdsPortal.Common
{
    using System;
    using System.Reflection;
    using AdsPortal.Common.Extensions;

    public static class AppInfo
    {
        public static Version AppVersion { get; } = Assembly.GetEntryAssembly()?.GetName()?.Version ?? new Version(0, 0, 0, 0);
        public static string AppVersionText { get; } = AppVersion.ToString();
        public static string AppVersionTextPlain { get; } = AppVersion.ToString().RemoveChar('.');

        public static string AppName { get; } = "AdsPortal";
        public static string Author { get; } = "Adam Bajguz";
        public static string AppDevelopmentYear { get; } = "2020";

        public static string AppCopyright { get; } = $"© {Author} {AppDevelopmentYear}";
        public static string AppNameWithVersion { get; } = $"{AppName} v{AppVersionText}";
        public static string AppNameWithVersionCopyright { get; } = $"{AppName} v{AppVersionText} — {AppCopyright}";
        public static string AppNameWithCopyright { get; } = $"{AppName} — {AppCopyright}";

        public static string InteractiveCLI { get; } = $"{AppName} CLI";
        public static string InteractiveCLIWithVersion { get; } = $"{AppName} CLI v{AppVersionText}";

        public static string SentryReleaseVersion { get; } = $"{AppName}-v{AppVersionText}";
        public static string SwaggerDocumentName { get; } = "v1";

        public static string SwaggerRoute { get; } = "api";
        public static string SwaggerStartupUrl { get; } = $"/{SwaggerRoute}/{SwaggerDocumentName}/swagger.json";
        public static string RestApiUrl { get; } = $"/{SwaggerRoute}";
        public static string SwaggerUrl { get; } = $"/{SwaggerRoute}/index.html";
        public static string HealthUrl { get; } = "/health";

        public static string AppShortDescription { get; } = $"Backend Api for {AppName}.";

        public static string AppDescription { get; } = $"{AppShortDescription}.\n" +
                                                       $"{AppCopyright}\n" +
                                                       "\n" +
                                                       "Links:\n" +
                                                       @$"OpenAPI specification can be found at {SwaggerStartupUrl}\n" +
                                                       @$"Swagger can be accessed through {SwaggerUrl}\n" +
                                                       "\n" +
                                                       @$"REST API base url is {RestApiUrl}\n" +
                                                       "\n" +
                                                       @$"App health can be checked under {HealthUrl}\n";

        public static string AppDescriptionHTML { get; } = $"Backend Api for {AppName}<br>" +
                                                           $"{AppCopyright}<br>" +
                                                           "<hr>" +
                                                           "<p>" +
                                                           "<b>Links:</b><br>" +
                                                           @$"OpenAPI specification can be found at <a href=""{SwaggerStartupUrl}"">{SwaggerStartupUrl}</a><br>" +
                                                           @$"Swagger can be accessed through <a href=""{SwaggerUrl}"">{SwaggerUrl}</a><br>" +
                                                           @$"<br>" +
                                                           @$"<a href=""/"">Go back to Portal</a><br>" +
                                                           @$"<br>" +
                                                           @$"REST API base url is <a href=""{RestApiUrl}"">{RestApiUrl}</a><br>" +
                                                           @$"<br>" +
                                                           @$"App health can be checked under <a href=""{HealthUrl}"">{HealthUrl}</a><br>" +
                                                           "</p>";
    }
}
