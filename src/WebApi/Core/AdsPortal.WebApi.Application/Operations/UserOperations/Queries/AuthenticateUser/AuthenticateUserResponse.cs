namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.AuthenticateUser
{
    using System;
    using MediatR.GenericOperations.Abstractions;

    public sealed record AuthenticateUserResponse : IOperationResult
    {
        public string Token { get; init; } = default!;
        public TimeSpan Lease { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
