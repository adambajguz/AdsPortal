namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public abstract class GetDetailsQueryHandler<TQuery, TEntity, TResult> : GetDetailsOperationHandler<TQuery, TEntity, TResult>
        where TQuery : class, IGetDetailsQuery<TResult>
        where TEntity : class, IBaseRelationalEntity
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {
        private TQuery? query;
        protected TQuery Query { get => query ?? throw new NullReferenceException("Handler not initialized properly"); private set => query = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalReadOnlyRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected GetDetailsQueryHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetReadOnlyRepository<TEntity>();
            Mapper = mapper;
        }

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
        public sealed override async Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Query = query = await OnInit(query, cancellationToken);

            await OnValidate(cancellationToken);
            TResult response = await OnFetch(cancellationToken);

            //TResult response = Mapper.Map<TResult>(response);
            response = await OnFetched(response, cancellationToken);

            return response;
        }
    }
}
