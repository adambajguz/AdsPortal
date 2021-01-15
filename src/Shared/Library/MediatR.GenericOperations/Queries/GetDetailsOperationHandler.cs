namespace MediatR.GenericOperations.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper.Extensions;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public interface IGetDetailsQuery<TResult> : IOperation<TResult>
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {

    }

    public abstract class GetDetailsOperationHandler<TQuery, TEntity, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : class, IGetDetailsQuery<TResult>
        where TEntity : class
        where TResult : class, IIdentifiableOperationResult, ICustomMapping
    {
        public abstract Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);

        protected virtual ValueTask<TQuery> OnInit(TQuery command, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(command);
        }

        protected virtual ValueTask OnValidate(CancellationToken cancellationToken)
        {
            return default;
        }

        protected abstract ValueTask<TResult> OnFetch(CancellationToken cancellationToken);

        protected virtual ValueTask<TResult> OnFetched(TResult response, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(response);
        }
    }
}
