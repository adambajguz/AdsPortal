namespace AdsPortal.WebApi.Infrastructure.Mailing.Services.Renderer
{
    using FluentEmail.Core.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class FluentEmailRazorBuilderExtensions
    {
        /// <summary>
        /// Add razor renderer with project views and layouts
        /// </summary>
        public static FluentEmailServicesBuilder AddRazorLightRenderer(this FluentEmailServicesBuilder builder, string templateRootFolder, bool inlineCss, bool inlineHtml)
        {
            builder.Services.AddSingleton<ITemplateRenderer>((provider) =>
            {
                ILogger<RazorRenderer> logger = provider.GetRequiredService<ILogger<RazorRenderer>>();

                return new RazorRenderer(templateRootFolder, logger, inlineCss, inlineHtml);
            });

            return builder;
        }
    }
}
