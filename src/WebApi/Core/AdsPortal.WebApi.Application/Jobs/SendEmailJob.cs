namespace AdsPortal.WebApi.Application.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Interfaces.Mailing;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public record SendEmailJobArguments
    {
        public string? Email { get; init; }
        public IEmailTemplate? Template { get; init; }
    }

    public class SendEmailJob : IJob
    {
        private readonly IEmailSenderService _emailSender;

        public SendEmailJob(IEmailSenderService emailSender)
        {
            _emailSender = emailSender;
        }

        public async ValueTask Handle(Guid jobId, object? args, CancellationToken cancellationToken)
        {
            if (args is SendEmailJobArguments emailArgs && emailArgs.Email is not null && emailArgs.Template is not null)
            {
                await _emailSender.SendEmailAsync(emailArgs.Email, emailArgs.Template, cancellationToken);
            }
        }
    }
}
