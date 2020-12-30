namespace AdsPortal.Infrastructure.Identity.Configurations
{
    using System;

    public sealed class JwtConfiguration
    {
        public string? Key { get; set; }
        public TimeSpan Lease { get; set; }
        public string? Issuer { get; set; }
    }
}
