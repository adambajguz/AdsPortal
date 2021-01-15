namespace MediatR.GenericOperations.Queries
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper.Extensions;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Models;

    public interface IGetListQuery<TResultEntry> : IOperation<ListResult<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {

    }

    public abstract class GetListOperationHandler<TQuery, TEntity, TResultEntry> : IRequestHandler<TQuery, ListResult<TResultEntry>>
        where TQuery : class, IGetListQuery<TResultEntry>
        where TEntity : class
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public abstract Task<ListResult<TResultEntry>> Handle(TQuery query, CancellationToken cancellationToken);

        protected virtual ValueTask<TQuery> OnInit(TQuery command, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(command);
        }

        protected virtual ValueTask OnValidate(CancellationToken cancellationToken)
        {
            return default;
        }

        protected abstract ValueTask<List<TResultEntry>> OnFetch(CancellationToken cancellationToken);

        protected virtual ValueTask<ListResult<TResultEntry>> OnFetched(ListResult<TResultEntry> response, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(response);
        }
    }
}
