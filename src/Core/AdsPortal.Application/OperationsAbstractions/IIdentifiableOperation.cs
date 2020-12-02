namespace AdsPortal.Application.OperationsAbstractions
{
    using System;

    public interface IIdentifiableOperation : IOperation
    {
        Guid Id { get; }
    }

    public interface IIdentifiableOperation<out TResult> : IOperation<TResult>
        where TResult : class, IIdentifiableOperationResult
    {
        Guid Id { get; }
    }
}