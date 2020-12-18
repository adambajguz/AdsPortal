namespace AdsPortal.Infrastructure.Email
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces;
    using AdsPortal.Infrastructure.Configurations;
    using Microsoft.Extensions.Options;

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _settings;

        public EmailService(IOptions<EmailConfiguration> settings)
        {
            _settings = settings.Value;
        }

        public virtual async Task SendEmail(string email, string subject, string message)
        {
            using (SmtpClient client = new SmtpClient(_settings.SmtpClient))
            {
                NetworkCredential credential = new NetworkCredential
                {
                    UserName = _settings.EmailAddress,
                    Password = _settings.Password
                };

                client.Credentials = credential;
                client.Host = _settings.Host;
                client.Port = _settings.Port;
                client.EnableSsl = true;

                using (MailMessage emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress(_settings.EmailAddress);
                    emailMessage.Subject = subject;
                    emailMessage.Body = WebUtility.HtmlDecode(message);
                    emailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(emailMessage);
                }
            }
        }
    }
}
