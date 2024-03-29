namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
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

        public sealed override async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command = await OnInit(command, cancellationToken);

            List<TEntity> entitiesToRemove = await Repository.AllAsync(Filter, cancellationToken: cancellationToken);

            foreach (TEntity entity in entitiesToRemove)
            {
                await OnValidate(entity, cancellationToken);
            }

            Repository.RemoveMultiple(entitiesToRemove);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnRemoved(cancellationToken);

            return await Unit.Task;
        }
    }
}
