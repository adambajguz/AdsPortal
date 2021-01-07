namespace MediatR.GenericOperations.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;

    public interface ICreateCommand : IOperation<IdResult>, ICustomMapping
    {

    }

    public abstract class CreateOperationHandler<TCommand, TEntity> : IRequestHandler<TCommand, IdResult>
        where TCommand : class, ICreateCommand
        where TEntity : class
    {
        public abstract Task<IdResult> Handle(TCommand command, CancellationToken cancellationToken);

        protected virtual ValueTask<TCommand> OnInit(TCommand command, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(command);
        }

        protected virtual ValueTask OnValidate(CancellationToken cancellationToken)
        {
            return default;
        }

        protected virtual ValueTask<TEntity> OnMapped(TEntity entity, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(entity);
        }

        protected virtual ValueTask OnAdded(TEntity entity, CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
