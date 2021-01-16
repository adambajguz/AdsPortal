namespace AdsPortal.WebApi.Application.Jobs
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Interfaces.Mailing;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public record SendEfmailJobArguments
    {
        public string? Email { get; init; }
        public IEmailTemplate? Template { get; init; }
    }

    public class SendEfmailJob : IJob
    {
        private readonly IEmailSenderService _emailSender;

        public SendEfmailJob(IEmailSenderService emailSender)
        {
            _emailSender = emailSender;
        }

        public async ValueTask Handle(object? args, CancellationToken cancellationToken)
        {
            if (args is SendEfmailJobArguments emailArgs && emailArgs.Email is not null && emailArgs.Template is not null)
            {
                await _emailSender.SendEmailAsync(emailArgs.Email, emailArgs.Template, cancellationToken);
            }
        }
    }
}
