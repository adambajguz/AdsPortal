namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Mapping;

    public interface IGetPagedListQuery<TResultEntry> : IOperation<PagedListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }
    }
}
