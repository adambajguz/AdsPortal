namespace AdsPortal.WebApi.Application.Interfaces.Mailing
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public interface IEmailSenderService
    {
        /// <summary>
        /// Sends raw email.
        /// </summary>
        Task SendEmailAsync(string emailAddress, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends email using template.
        /// </summary>
        Task SendEmailAsync<T>(string emailAddress, T template, CancellationToken cancellationToken = default)
            where T : IEmailTemplate;
    }
}
