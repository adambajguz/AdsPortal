namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Abstractions.Base;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Queries;

    public abstract class GetDetailsByIdQueryHandler<TQuery, TEntity, TResult> : GetDetailsOperationHandler<TQuery, TEntity, TResult>
        where TQuery : class, IGetDetailsQuery<TResult>, IIdentifiableOperation<TResult>
        where TEntity : class, IBaseRelationalEntity
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {
        private TQuery? query;
        protected TQuery Query { get => query ?? throw new NullReferenceException("Handler not initialized properly"); private set => query = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected GetDetailsByIdQueryHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
            Mapper = mapper;
        }

        public override async Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Query = query;
            await OnInit(cancellationToken);

            TEntity entity = await OnFetch(cancellationToken);
            await OnValidate(entity, cancellationToken);

            TResult response = Mapper.Map<TResult>(entity); //TODO: Consider using ProjectTo in repository instead of Map
            await OnMapped(entity, response, cancellationToken);

            return response;
        }

        protected override async Task<TEntity> OnFetch(CancellationToken cancellationToken)
        {
            return await Repository.SingleByIdAsync(Query.Id, noTracking: true, cancellationToken);
        }
    }
}
