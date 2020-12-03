namespace AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;

    public class JwtTokenModel : IOperationResult
    {
        public string Token { get; init; } = default!;
        public TimeSpan Lease { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
