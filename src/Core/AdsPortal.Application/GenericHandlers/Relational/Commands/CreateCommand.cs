namespace AdsPortal.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Abstractions.Base;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using FluentValidation;
    using MediatR;

    public interface ICreateCommand : IOperation<IdResult>, ICustomMapping
    {

    }

    public abstract class CreateCommandHandler<TCommand, TCommandValidator, TEntity> : IRequestHandler<TCommand, IdResult>
        where TCommand : class, ICreateCommand
        where TCommandValidator : AbstractValidator<TCommand>, new()
        where TEntity : class, IBaseRelationalEntity
    {
        private TCommand? command;
        protected TCommand Command { get => command ?? throw new NullReferenceException("Handler not initialized properly"); private set => command = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected CreateCommandHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
            Mapper = mapper;
        }

        public async Task<IdResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            await new TCommandValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);
            await OnValidate(cancellationToken);

            TEntity entity = Mapper.Map<TEntity>(command);
            await OnMapped(entity, cancellationToken);

            Repository.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnAdded(entity, cancellationToken);

            return new IdResult { Id = entity.Id };
        }

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

    public abstract class CreateCommandHandler<TCommand, TEntity> : IRequestHandler<TCommand, IdResult>
        where TCommand : class, ICreateCommand
        where TEntity : class, IBaseRelationalEntity
    {
        private TCommand? command;
        protected TCommand Command { get => command ?? throw new NullReferenceException("Handler not initialized properly"); private set => command = value; }

        protected IAppRelationalUnitOfWork Uow { get; }
        protected IGenericRelationalRepository<TEntity> Repository { get; }
        protected IMapper Mapper { get; }

        protected CreateCommandHandler(IAppRelationalUnitOfWork uow, IMapper mapper)
        {
            Uow = uow;
            Repository = Uow.GetRepository<TEntity>();
            Mapper = mapper;
        }

        public async Task<IdResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command;
            await OnInit(cancellationToken);

            await OnValidate(cancellationToken);

            TEntity entity = Mapper.Map<TEntity>(command);
            await OnMapped(entity, cancellationToken);

            Repository.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnAdded(entity, cancellationToken);

            return new IdResult { Id = entity.Id };
        }

        protected abstract Task OnInit(CancellationToken cancellationToken);

        protected abstract Task OnValidate(CancellationToken cancellationToken);

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
