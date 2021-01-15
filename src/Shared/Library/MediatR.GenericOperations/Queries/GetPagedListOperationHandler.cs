namespace MediatR.GenericOperations.Queries
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper.Extensions;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
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

        protected virtual ValueTask<TQuery> OnInit(TQuery command, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(command);
        }

        protected virtual ValueTask OnValidate(CancellationToken cancellationToken)
        {
            return default;
        }

        protected abstract ValueTask<List<TResultEntry>> OnFetch(int skip, int entriesPerPage, int total, CancellationToken cancellationToken);

        protected virtual ValueTask<PagedListResult<TResultEntry>> OnFetched(PagedListResult<TResultEntry> response, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(response);
        }
    }
}
