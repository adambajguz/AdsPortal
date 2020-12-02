namespace AdsPortal.Application.OperationsAbstractions
{
    using System;

    public interface IIdentifiableOperationResult : IOperationResult
    {
        Guid Id { get; }
    }
}
