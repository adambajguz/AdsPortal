namespace AdsPortal.WebApi.Domain.EmailTemplates
{
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public class AccountRegisteredEmail : IEmailTemplate
    {
        public string Subject => "Welcome to AdsPortal";
        public string ViewPath => "AccountRegistered.cshtml";

        public string? Name { get; init; }
    }
}
