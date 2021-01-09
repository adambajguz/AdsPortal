namespace AdsPortal.Infrastructure.Configurations
{
    public class EmailConfiguration
    {
        public string? DefaultFromEmail { get; init; }
        public string? DefaultFromName { get; init; }

        /// <summary>
        /// Whether CSS are moved to inline attributes using PreMailer.Net, to gain maximum E-mail client compatibility.
        /// </summary>
        public bool InlineCss { get; init; }

        /// <summary>
        /// Whether to remove '\r', '\n', and '\t'.
        /// </summary>
        public bool InlineHtml { get; init; }

        public EmailSmtpConfiguration? SmtpConfiguration { get; init; }
    }

    public class EmailSmtpConfiguration
    {
        public bool Enabled { get; init; }
        public string? SmtpHost { get; init; }
        public int SmtpPort { get; init; }
        public string? SmtpUsername { get; init; }
        public string? SmtpPassword { get; init; }
    }
}
