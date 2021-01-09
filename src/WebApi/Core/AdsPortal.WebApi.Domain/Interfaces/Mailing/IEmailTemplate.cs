namespace AdsPortal.WebApi.Domain.Interfaces.Mailing
{
    public interface IEmailTemplate
    {
        /// <summary>
        /// Email subject.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Email template view path.
        /// </summary>
        public string ViewPath { get; }
    }
}
