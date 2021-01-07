namespace MediatR.GenericOperations.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

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

        protected abstract ValueTask<TEntity> OnFetch(CancellationToken cancellationToken);

        protected virtual ValueTask OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return default;
        }

        protected virtual ValueTask<TResult> OnMapped(TEntity entity, TResult response, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(response);
        }
    }
}
