namespace AdsPortal.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Abstractions.Base;
    using MediatR;

    public interface IDeleteMultipleCommand : IOperation
    {

    }

    public abstract class DeleteMultipleHandler<TCommand, TEntity> : IRequestHandler<TCommand, Unit>
        where TCommand : class, IDeleteMultipleCommand
        where TEntity : class, IBaseRelationalEntity
    {
        private TCommand? command;
        protected TCommand Command { get => command ?? throw new NullReferenceException("Handler not initialized properly"); private set => command = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }

        protected Expression<Func<TEntity, bool>>? Filter { get; set; } = null;

        protected DeleteMultipleHandler(IAppRelationalUnitOfWork uow)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
        }

        public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            List<TEntity> entitiesToRemove = await Repository.AllAsync(Filter, cancellationToken: cancellationToken);
            await OnValidate(entitiesToRemove, cancellationToken);

            Repository.RemoveMultiple(entitiesToRemove);
            await Uow.SaveChangesAsync();
            await OnRemoved(cancellationToken);

            return await Unit.Task;
        }

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(List<TEntity> entitiesToRemove, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnRemoved(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
