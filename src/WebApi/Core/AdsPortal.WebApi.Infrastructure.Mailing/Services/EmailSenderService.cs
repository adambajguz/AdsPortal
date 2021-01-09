namespace AdsPortal.WebApi.Infrastructure.Mailing.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Mailing;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;
    using AdsPortal.WebApi.Infrastructure.Mailing.Services.Renderer;
    using FluentEmail.Core;
    using Microsoft.Extensions.Logging;

    public class EmailSenderService : IEmailSenderService
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly ILogger _logger;

        public EmailSenderService(IFluentEmail fluentEmail, ILogger<EmailSenderService> logger)
        {
            _fluentEmail = fluentEmail;
            _logger = logger;
        }

        public async Task SendEmailAsync(string emailAddress, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default)
        {
            var response = await _fluentEmail.To(emailAddress)
                                             .Subject(subject)
                                             .Body(body, isHtml)
                                             .SendAsync(cancellationToken);

            if (response.Successful)
                _logger.LogInformation("Successfully send email ({MessageId}, {Subject}).", response.MessageId, subject);
            else
                _logger.LogError("Failed to send email ({Subject}). {Response}", subject, response);
        }

        public async Task SendEmailAsync<T>(string emailAddress, T template, CancellationToken cancellationToken = default)
            where T : IEmailTemplate
        {
            var response = await _fluentEmail.To(emailAddress)
                                             .Subject(template.Subject)
                                             .UsingRazorTemplate(template)
                                             .SendAsync(cancellationToken);

            if (response.Successful)
                _logger.LogInformation("Successfully send email ({MessageId}, {Subject}, {ViewPath}).", response.MessageId, template.Subject, template.ViewPath);
            else
                _logger.LogError("Failed to send email ({Subject}, {ViewPath}). {Response}", template.Subject, template.ViewPath, response);
        }
    }
}
