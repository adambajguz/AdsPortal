namespace AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AutoMapper;
    using FluentValidation;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Models;

    public abstract class CreateCommandHandler<TCommand, TCommandValidator, TEntity> : CreateOperationHandler<TCommand, TEntity>
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

        //TODO: maybe handle should be in library
        public sealed override async Task<IdResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command = await OnInit(command, cancellationToken);

            await new TCommandValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);
            await OnValidate(cancellationToken);

            TEntity entity = Mapper.Map<TEntity>(command);
            entity = await OnMapped(entity, cancellationToken);

            Repository.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnAdded(entity, cancellationToken);

            return new IdResult { Id = entity.Id };
        }
    }

    public abstract class CreateCommandHandler<TCommand, TEntity> : CreateOperationHandler<TCommand, TEntity>
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

        public override async Task<IdResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Command = command = await OnInit(command, cancellationToken);

            await OnValidate(cancellationToken);

            TEntity entity = Mapper.Map<TEntity>(command);
            entity = await OnMapped(entity, cancellationToken);

            Repository.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
            await OnAdded(entity, cancellationToken);

            return new IdResult { Id = entity.Id };
        }
    }
}
