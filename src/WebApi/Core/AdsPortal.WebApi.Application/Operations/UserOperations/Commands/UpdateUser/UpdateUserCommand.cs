namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.UpdateUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Constants;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using AutoMapper;
    using FluentValidation;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;
    using Newtonsoft.Json;

    public sealed record UpdateUserCommand : IUpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public string? Email { get; init; }

        public string? Name { get; init; }
        public string? Surname { get; init; }

        public Roles Role { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateUserCommand, User>();
        }

        private sealed class Handler : UpdateCommandHandler<UpdateUserCommand, User>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override async ValueTask OnValidate(User entity, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrow(Command.Id);

                //TODO: add more generic approach
                if (Command.Role.HasFlag(Roles.Admin))
                {
                    _drs.IsAdminElseThrow();
                }

                if (Command.Role.HasFlag(Roles.User))
                {
                    _drs.HasRole(Roles.User);
                }

                await new UpdateUserValidator().ValidateAndThrowAsync(Command, cancellationToken: cancellationToken);

                if (Command.Email != entity.Email && await Uow.Users.IsEmailInUseAsync(Command.Email))
                {
                    throw new ValidationFailedException(nameof(Command.Email), ValidationMessages.Email.IsInUse);
                }
            }
        }
    }
}
