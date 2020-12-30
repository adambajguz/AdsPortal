namespace MediatR.GenericOperations.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public interface IDeleteCommand : IOperation
    {

    }

    public abstract class DeleteOperationHandler<TCommand, TEntity> : IRequestHandler<TCommand, Unit>
        where TCommand : class, IDeleteCommand
        where TEntity : class
    {
        public abstract Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnRemoved(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
