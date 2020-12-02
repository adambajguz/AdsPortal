namespace AdsPortal.Infrastructure.Identity.Jwt
{
    using System;

    public sealed class JwtSettings
    {
        public string? Key { get; set; }
        public TimeSpan Lease { get; set; }
        public string? Issuer { get; set; }
    }
}
