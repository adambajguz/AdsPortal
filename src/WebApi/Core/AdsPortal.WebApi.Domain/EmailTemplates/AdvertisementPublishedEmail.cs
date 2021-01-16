namespace AdsPortal.WebApi.Domain.EmailTemplates
{
    using System;
    using AdsPortal.WebApi.Domain.Interfaces.Mailing;

    public class AdvertisementPublishedEmail : IEmailTemplate
    {
        public string Subject => $"Your Ad '{AdvertisementTitle}' was published on AdsPortal";
        public string ViewPath => "AdvertisementPublished.cshtml";

        public string? UserName { get; init; }
        public string? AdvertisementTitle { get; init; }
        public DateTime AdvertisementVisibleTo { get; init; }
    }
}
