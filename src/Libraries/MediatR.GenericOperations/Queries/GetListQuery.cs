namespace MediatR.GenericOperations.Queries
{
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;

    public interface IGetListQuery<TResultEntry> : IOperation<ListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {

    }
}
