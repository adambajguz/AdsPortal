namespace AdsPortal.WebApi.Domain.EmailTemplates
{
    using System;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public class AdvertisementAboutToExpireEmail : IEmailTemplate
    {
        public string Subject => $"Your Ad '{AdvertisementTitle}' will expire soon";
        public string ViewPath => "AdvertisementAboutToExpireEmail.cshtml";

        public string? UserName { get; init; }
        public string? AdvertisementTitle { get; init; }
        public DateTime AdvertisementVisibleTo { get; init; }
    }
}
