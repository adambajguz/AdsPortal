namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;

    public interface IGetPagedListQuery<TResultEntry> : IOperation<PagedListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }
    }

    public abstract class GetPagedListOperationHandler<TQuery, TEntity, TResultEntry> : IRequestHandler<TQuery, PagedListResult<TResultEntry>>
       where TQuery : class, IGetPagedListQuery<TResultEntry>
       where TEntity : class
       where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public abstract Task<PagedListResult<TResultEntry>> Handle(TQuery query, CancellationToken cancellationToken);

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected abstract Task<List<TResultEntry>> OnFetch(int skip, int entriesPerPage, int total, CancellationToken cancellationToken);

        protected virtual Task OnFetched(PagedListResult<TResultEntry> response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
