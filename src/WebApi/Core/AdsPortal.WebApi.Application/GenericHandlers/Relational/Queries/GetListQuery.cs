namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Models;
    using MediatR.GenericOperations.Queries;

    public abstract class GetListQueryHandler<TQuery, TEntity, TResultEntry> : GetListOperationHandler<TQuery, TEntity, TResultEntry>
        where TQuery : class, IGetListQuery<TResultEntry>
        where TEntity : class, IBaseRelationalEntity
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        private TQuery? query;
        protected TQuery Query { get => query ?? throw new NullReferenceException("Handler not initialized properly"); private set => query = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }

        protected Expression<Func<TEntity, bool>>? Filter { get; set; } = null;
        protected Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; set; } = null;

        protected GetListQueryHandler(IAppRelationalUnitOfWork uow)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
        }

        [SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value")]
        public sealed override async Task<ListResult<TResultEntry>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Query = query = await OnInit(query, cancellationToken);
            await OnValidate(cancellationToken);

            List<TResultEntry> list = await OnFetch(cancellationToken);

            ListResult<TResultEntry> response = new ListResult<TResultEntry>(list);
            response = await OnFetched(response, cancellationToken);

            return response;
        }

        protected override async ValueTask<List<TResultEntry>> OnFetch(CancellationToken cancellationToken)
        {
            return await Repository.ProjectedAllAsync<TResultEntry>(filter: Filter,
                                                                    orderBy: OrderBy,
                                                                    noTracking: true,
                                                                    cancellationToken: cancellationToken);
        }
    }
}
