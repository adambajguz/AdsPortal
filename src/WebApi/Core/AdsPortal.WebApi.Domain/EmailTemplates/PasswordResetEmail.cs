namespace AdsPortal.WebApi.Domain.EmailTemplates
{
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public class ResetPasswordEmail : IEmailTemplate
    {
        public string Subject => "Reset password";
        public string ViewPath => "ResetPassword.cshtml";

        public string? CallbackUrl { get; init; }
    }
}
