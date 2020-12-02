namespace AdsPortal.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Abstractions.Base;
    using MediatR;

    public interface IDeleteByIdCommand : IIdentifiableOperation
    {

    }

    public abstract class DeleteByIdHandler<TCommand, TEntity> : IRequestHandler<TCommand, Unit>
        where TCommand : class, IDeleteByIdCommand
        where TEntity : class, IBaseRelationalEntity
    {
        private TCommand? command;
        protected TCommand Command { get => command ?? throw new NullReferenceException("Handler not initialized properly"); private set => command = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }

        protected DeleteByIdHandler(IAppRelationalUnitOfWork uow)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
        }

        public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            Guid id = command.Id;
            TEntity entity = await Repository.SingleByIdAsync(id, cancellationToken: cancellationToken);
            await OnValidate(entity, cancellationToken);

            Repository.Remove(entity);
            await Uow.SaveChangesAsync();
            await OnRemoved(cancellationToken);

            return await Unit.Task;
        }

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
