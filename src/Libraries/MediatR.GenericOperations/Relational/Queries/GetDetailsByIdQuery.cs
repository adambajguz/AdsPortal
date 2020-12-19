namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;

    public interface IGetDetailsByIdQuery<TResult> : IIdentifiableOperation<TResult>
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {

    }
}
