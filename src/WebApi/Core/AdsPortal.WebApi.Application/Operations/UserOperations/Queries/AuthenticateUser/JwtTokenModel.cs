namespace AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken
{
    using System;
    using MediatR.GenericOperations.Abstractions;

    public sealed record JwtTokenModel : IOperationResult
    {
        public string Token { get; init; } = default!;
        public TimeSpan Lease { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
