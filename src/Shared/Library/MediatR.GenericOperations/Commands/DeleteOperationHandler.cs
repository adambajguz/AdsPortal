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

        protected virtual ValueTask<TCommand> OnInit(TCommand command, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(command);
        }

        protected virtual ValueTask OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return default;
        }

        protected virtual ValueTask OnRemoved(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
