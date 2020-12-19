namespace MediatR.GenericOperations.Queries
{
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;

    public interface IGetPagedListQuery<TResultEntry> : IOperation<PagedListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }
    }
}
