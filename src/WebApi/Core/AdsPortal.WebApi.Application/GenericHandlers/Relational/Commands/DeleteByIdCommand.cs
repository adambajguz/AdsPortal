namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Commands;

    public abstract class DeleteByIdHandler<TCommand, TEntity> : DeleteOperationHandler<TCommand, TEntity>
        where TCommand : class, IDeleteCommand, IIdentifiableOperation
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

        public override async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command = await OnInit(command, cancellationToken);

            Guid id = command.Id;
            TEntity entity = await Repository.SingleByIdAsync(id, cancellationToken: cancellationToken);
            await OnValidate(entity, cancellationToken);

            Repository.Remove(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnRemoved(cancellationToken);

            return await Unit.Task;
        }
    }
}
