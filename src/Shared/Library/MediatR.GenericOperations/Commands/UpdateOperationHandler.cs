namespace MediatR.GenericOperations.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper.Extensions;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public interface IUpdateCommand : IIdentifiableOperation, ICustomMapping
    {

    }

    public abstract class UpdateOperationHandler<TCommand, TEntity> : IRequestHandler<TCommand, Unit>
        where TCommand : class, IUpdateCommand
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

        protected virtual ValueTask<TEntity> OnMapped(TEntity entity, CancellationToken cancellationToken)
        {
            return ValueTask.FromResult(entity);
        }

        protected virtual ValueTask OnUpdated(TEntity entity, CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
