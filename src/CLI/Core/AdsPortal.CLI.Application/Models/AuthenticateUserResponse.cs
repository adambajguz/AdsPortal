namespace AdsPortal.CLI.Application.Models
{
    using System;

    public record AuthenticateUserResponse
    {
        public string Token { get; init; } = default!;
        public TimeSpan Lease { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
