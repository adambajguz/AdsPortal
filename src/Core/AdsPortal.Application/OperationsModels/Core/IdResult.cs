namespace AdsPortal.Application.OperationsModels.Core
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;

    public class IdResult : IIdentifiableOperationResult
    {
        public Guid Id { get; init; }
    }
}