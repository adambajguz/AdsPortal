namespace MediatR.GenericOperations.Models
{
    using System;
    using MediatR.GenericOperations.Abstractions;

    public class IdResult : IIdentifiableOperationResult
    {
        public Guid Id { get; init; }
    }
}