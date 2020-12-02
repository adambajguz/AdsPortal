namespace AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;

    public class JwtTokenModel : IOperationResult
    {
        public string Token { get; set; } = default!;
        public TimeSpan Lease { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
