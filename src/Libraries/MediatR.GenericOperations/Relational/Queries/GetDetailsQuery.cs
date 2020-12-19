namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;

    public interface IGetDetailsQuery<TResult> : IOperation<TResult>
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {

    }
}
