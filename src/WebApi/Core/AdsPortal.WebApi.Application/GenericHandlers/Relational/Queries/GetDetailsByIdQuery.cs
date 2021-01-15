namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public abstract class GetDetailsByIdQueryHandler<TQuery, TEntity, TResult> : GetDetailsQueryHandler<TQuery, TEntity, TResult>
        where TQuery : class, IGetDetailsQuery<TResult>, IIdentifiableOperation<TResult>
        where TEntity : class, IBaseRelationalEntity
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {
        protected GetDetailsByIdQueryHandler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {

        }

        protected override async ValueTask<TResult> OnFetch(CancellationToken cancellationToken)
        {
            return await Repository.ProjectedSingleByIdAsync<TResult>(Query.Id, noTracking: true, cancellationToken);
        }
    }
}
