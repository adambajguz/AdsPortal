namespace MediatR.GenericOperations.Queries
{
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public interface IGetDetailsQuery<TResult> : IOperation<TResult>
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {

    }
}
