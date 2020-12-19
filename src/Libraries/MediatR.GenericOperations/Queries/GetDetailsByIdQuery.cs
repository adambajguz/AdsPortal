namespace MediatR.GenericOperations.Queries
{
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public interface IGetDetailsByIdQuery<TResult> : IIdentifiableOperation<TResult>
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {

    }
}
