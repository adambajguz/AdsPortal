namespace AdsPortal.WebApi.Infrastructure.Mailing
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using AdsPortal.Infrastructure.Configurations;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Mailing;
    using AdsPortal.WebApi.Infrastructure.Mailing.Services;
    using AdsPortal.WebApi.Infrastructure.Mailing.Services.Renderer;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration configuration, string emailTemplatesRootFolder)
        {
            services.AddConfiguration<EmailConfiguration>(configuration, out EmailConfiguration emailOptions);
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            if (string.IsNullOrWhiteSpace(emailTemplatesRootFolder))
            {
                emailTemplatesRootFolder = Path.Combine(Path.GetFullPath("Views"),
                                                        string.Format("Shared{0}EmailTemplates{0}", Path.DirectorySeparatorChar));
            }
            else if (!emailTemplatesRootFolder.EndsWith(Path.DirectorySeparatorChar) && !emailTemplatesRootFolder.EndsWith(Path.AltDirectorySeparatorChar))
            {
                emailTemplatesRootFolder += Path.DirectorySeparatorChar;
            }

            var fluentEmailBuilder = services
                .AddFluentEmail(emailOptions.DefaultFromEmail ?? throw new NullReferenceException($"Invalid email configuration. {nameof(EmailConfiguration.DefaultFromEmail)} cannot be null."),
                                emailOptions.DefaultFromName ?? throw new NullReferenceException($"Invalid email configuration. {nameof(EmailConfiguration.DefaultFromName)} cannot be null."))
                .AddRazorLightRenderer(emailTemplatesRootFolder, emailOptions.InlineCss, emailOptions.InlineHtml);

            if (emailOptions.SmtpConfiguration?.Enabled ?? false)
            {
                var smtpConfig = emailOptions.SmtpConfiguration;

                if (string.IsNullOrWhiteSpace(smtpConfig.SmtpUsername))
                {
                    fluentEmailBuilder.AddSmtpSender(smtpConfig.SmtpHost, smtpConfig.SmtpPort);
                }
                else
                {
                    fluentEmailBuilder.AddSmtpSender(() =>
                    {
                        SmtpClient client = new(smtpConfig.SmtpHost);

                        client.Host = smtpConfig.SmtpHost ?? throw new NullReferenceException($"Invalid email configuration. {nameof(EmailSmtpConfiguration.SmtpHost)} cannot be null.");
                        client.Port = smtpConfig.SmtpPort;
                        client.EnableSsl = true;

                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential
                        {
                            UserName = smtpConfig.SmtpUsername,
                            Password = smtpConfig.SmtpPassword
                        };

                        return client;
                    });
                }
            }

            return services;
        }
    }
}
