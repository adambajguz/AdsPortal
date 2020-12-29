namespace MediatR.GenericOperations.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class UpdateOperationHandler<TCommand, TEntity> : IRequestHandler<TCommand, Unit>
        where TCommand : class, IUpdateCommand
        where TEntity : class
    {
        public abstract Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnMapped(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnUpdated(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
