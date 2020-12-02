namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Mapping;
    using FluentValidation;
    using MediatR;

    public interface IGetPagedListQuery<TResultEntry> : IOperation<PagedListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }
    }

    public abstract class GetPagedListQueryHandler<TQuery, TEntity, TResultEntry> : IRequestHandler<TQuery, PagedListResult<TResultEntry>>
        where TQuery : class, IGetPagedListQuery<TResultEntry>
        where TEntity : class, IBaseRelationalEntity
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        private TQuery? query;
        protected TQuery Query { get => query ?? throw new NullReferenceException("Handler not initialized properly"); private set => query = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }

        protected Expression<Func<TEntity, bool>>? Filter { get; set; } = null;
        protected Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; set; } = null;

        protected GetPagedListQueryHandler(IAppRelationalUnitOfWork uow)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
        }

        public async Task<PagedListResult<TResultEntry>> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Query = query;
            await OnInit(cancellationToken);

            await new GetPagedListQueryValidator<TResultEntry>().ValidateAndThrowAsync(query, cancellationToken: cancellationToken);
            await OnValidate(cancellationToken);

            int entriesPerPage = query.EntiresPerPage;
            int pageNumber = query.Page;
            int skip = entriesPerPage * pageNumber;

            int total = await Repository.GetCountAsync(Filter);
            List<TResultEntry> list = await OnFetch(skip, entriesPerPage, total, cancellationToken);

            PagedListResult<TResultEntry> getPagedListResponse = new PagedListResult<TResultEntry>(pageNumber, query.EntiresPerPage, total, list);
            await OnFetched(getPagedListResponse, cancellationToken);

            return getPagedListResponse;
        }

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task<List<TResultEntry>> OnFetch(int skip, int entriesPerPage, int total, CancellationToken cancellationToken)
        {
            return await Repository.ProjectToAsync<TResultEntry>(filter: Filter,
                                                              orderBy: OrderBy,
                                                              noTracking: true,
                                                              skip: skip,
                                                              take: entriesPerPage,
                                                              cancellationToken: cancellationToken);
        }

        protected virtual Task OnFetched(PagedListResult<TResultEntry> response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
