namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Mapping;

    public interface IGetListQuery<TResultEntry> : IOperation<ListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {

    }
}
