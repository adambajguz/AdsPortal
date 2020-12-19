namespace MediatR.GenericOperations.Abstractions
{
    using System;

    public interface IIdentifiableOperationResult : IOperationResult
    {
        Guid Id { get; init; }
    }
}
