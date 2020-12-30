namespace MediatR.GenericOperations.Queries
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
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

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected abstract Task<List<TResultEntry>> OnFetch(CancellationToken cancellationToken);

        protected virtual Task OnFetched(ListResult<TResultEntry> response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
