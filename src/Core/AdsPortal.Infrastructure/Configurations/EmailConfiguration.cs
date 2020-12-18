namespace AdsPortal.Infrastructure.Configurations
{
    public sealed class EmailConfiguration
    {
        public string? SmtpClient { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
    }
}
