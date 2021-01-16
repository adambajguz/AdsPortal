namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper.Extensions;
    using FluentValidation;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Models;
    using MediatR.GenericOperations.Queries;

    public abstract class GetPagedListQueryHandler<TQuery, TEntity, TResultEntry> : GetPagedListOperationHandler<TQuery, TEntity, TResultEntry>
        where TQuery : class, IGetPagedListQuery<TResultEntry>
        where TEntity : class, IBaseRelationalEntity
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        private TQuery? query;
        protected TQuery Query { get => query ?? throw new NullReferenceException("Handler not initialized properly"); private set => query = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalReadOnlyRepository<TEntity> Repository { get; }

        protected Expression<Func<TEntity, bool>>? Filter { get; set; } = null;
        protected Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; set; } = null;

        protected GetPagedListQueryHandler(IAppRelationalUnitOfWork uow)
        {
            Uow = uow;
            Repository = Uow.GetReadOnlyRepository<TEntity>();
        }

        public sealed override async Task<PagedListResult<TResultEntry>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Query = query = await OnInit(query, cancellationToken);

            await new GetPagedListQueryValidator<TResultEntry>().ValidateAndThrowAsync(query, cancellationToken: cancellationToken);
            await OnValidate(cancellationToken);

            int entriesPerPage = query.EntiresPerPage;
            int pageNumber = query.Page;
            int skip = entriesPerPage * pageNumber;
            int total = await Repository.GetCountAsync(Filter, cancellationToken);

            List<TResultEntry> list = await OnFetch(skip, entriesPerPage, total, cancellationToken);

            PagedListResult<TResultEntry> response = new(pageNumber, query.EntiresPerPage, total, list);
            response = await OnFetched(response, cancellationToken);

            return response;
        }

        protected override async ValueTask<List<TResultEntry>> OnFetch(int skip, int entriesPerPage, int total, CancellationToken cancellationToken)
        {
            return await Repository.ProjectedAllAsync<TResultEntry>(filter: Filter,
                                                                    orderBy: OrderBy,
                                                                    noTracking: true,
                                                                    skip: skip,
                                                                    take: entriesPerPage,
                                                                    cancellationToken: cancellationToken);
        }
    }
}
