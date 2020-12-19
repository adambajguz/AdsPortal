namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Abstractions.Base;
    using AutoMapper;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Queries;

    public abstract class GetDetailsQueryHandler<TQuery, TEntity, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : class, IGetDetailsQuery<TResult>
        where TEntity : class, IBaseRelationalEntity
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {
        private TQuery? query;
        protected TQuery Query { get => query ?? throw new NullReferenceException("Handler not initialized properly"); private set => query = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected GetDetailsQueryHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
            Mapper = mapper;
        }

        public async Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Query = query;
            await OnInit(cancellationToken);

            TEntity entity = await OnFetch(cancellationToken);
            await OnValidate(entity, cancellationToken);

            TResult response = Mapper.Map<TResult>(entity); //TODO: Consider using ProjectTo in repository instead of Map
            await OnMapped(entity, response, cancellationToken);

            return response;
        }

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected abstract Task<TEntity> OnFetch(CancellationToken cancellationToken);

        protected virtual Task OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnMapped(TEntity entity, TResult response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
