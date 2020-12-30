namespace AdsPortal.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Abstractions.Base;
    using MediatR;
    using MediatR.GenericOperations.Commands;

    public abstract class DeleteMultipleHandler<TCommand, TEntity> : DeleteOperationHandler<TCommand, TEntity>
        where TCommand : class, IDeleteCommand
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

        public override async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            List<TEntity> entitiesToRemove = await Repository.AllAsync(Filter, cancellationToken: cancellationToken);

            foreach (TEntity entity in entitiesToRemove)
            {
                await OnValidate(entity, cancellationToken);
            }

            Repository.RemoveMultiple(entitiesToRemove);
            await Uow.SaveChangesAsync();
            await OnRemoved(cancellationToken);

            return await Unit.Task;
        }
    }
}
