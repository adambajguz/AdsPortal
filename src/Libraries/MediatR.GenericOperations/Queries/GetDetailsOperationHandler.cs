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

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected abstract Task<TEntity> OnFetch(CancellationToken cancellationToken);

        protected virtual Task OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnMapped(TEntity entity, TResult response, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
