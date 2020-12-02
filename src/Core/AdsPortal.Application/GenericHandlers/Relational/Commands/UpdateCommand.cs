namespace AdsPortal.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using FluentValidation;
    using MediatR;

    public interface IUpdateCommand : IIdentifiableOperation, ICustomMapping
    {

    }

    public abstract class UpdateCommandHandler<TCommand, TCommandValidator, TEntity> : IRequestHandler<TCommand, Unit>
        where TCommand : class, IUpdateCommand
        where TEntity : class, IBaseRelationalEntity
        where TCommandValidator : AbstractValidator<TCommand>, new()
    {
        private TCommand? command;
        protected TCommand Command { get => command ?? throw new NullReferenceException("Handler not initialized properly"); private set => command = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected UpdateCommandHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
            Mapper = mapper;
        }

        public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            TEntity entity = await Repository.SingleByIdAsync(command.Id, cancellationToken: cancellationToken);
            await new TCommandValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);
            await OnValidate(entity, cancellationToken);

            Mapper.Map(command, entity);
            await OnMapped(entity, cancellationToken);

            Repository.Update(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnUpdated(entity, cancellationToken);

            return await Unit.Task;
        }

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected virtual Task OnValidate(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnMapped(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnUpdated(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class UpdateCommandHandler<TCommand, TEntity> : IRequestHandler<TCommand, Unit>
            where TCommand : class, IUpdateCommand
            where TEntity : class, IBaseRelationalEntity
    {
        private TCommand? command;
        protected TCommand Command { get => command ?? throw new NullReferenceException("Handler not initialized properly"); private set => command = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected UpdateCommandHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
            Mapper = mapper;
        }

        public async Task<Unit> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            TEntity entity = await Repository.SingleByIdAsync(command.Id, cancellationToken: cancellationToken);
            await OnValidate(entity, cancellationToken);

            Mapper.Map(command, entity);
            await OnMapped(entity, cancellationToken);

            Repository.Update(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnUpdated(entity, cancellationToken);

            return await Unit.Task;
        }

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected abstract Task OnValidate(TEntity entity, CancellationToken cancellationToken);

        protected virtual Task OnMapped(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnUpdated(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
