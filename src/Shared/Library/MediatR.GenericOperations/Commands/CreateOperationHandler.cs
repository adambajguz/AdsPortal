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

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnMapped(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAdded(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
