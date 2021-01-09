namespace AdsPortal.WebApi.Infrastructure.Mailing.Services.Renderer
{
    using System;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;
    using FluentEmail.Core;

    public static class IFluentEmailExtensions
    {
        /// <summary>
        /// Based on: https://github.com/lukencode/FluentEmail/blob/master/src/FluentEmail.Core/Email.cs#L305
        /// For RazorLight based on embedded resources there is no need to read assembly ourself like in original project
        /// </summary>
        public static IFluentEmail UsingRazorTemplate<T>(this IFluentEmail email, T template)
            where T : IEmailTemplate
        {
            return email.UsingRazorTemplate(template.ViewPath, template);
        }

        /// <summary>
        /// Based on: https://github.com/lukencode/FluentEmail/blob/master/src/FluentEmail.Core/Email.cs#L305
        /// For RazorLight based on embedded resources there is no need to read assembly ourself like in original project
        /// </summary>
        public static IFluentEmail UsingRazorTemplate<T>(this IFluentEmail email, string path, T model)
        {
            const bool isHtml = true;

            if (email.Renderer is RazorRenderer renderer)
            {
                string result = renderer.Parse(path, model, isHtml);

                email.Data.IsHtml = isHtml;
                email.Data.Body = result;

                return email;
            }

            throw new InvalidOperationException($"Only {nameof(RazorRenderer)} renderer is supported");
        }
    }
}
