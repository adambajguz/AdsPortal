namespace AdsPortal.Application.Operations.UserOperations.Commands.UpdateUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Constants;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using FluentValidation;

    public class UpdateUserCommand : IUpdateCommand
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public Roles Role { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateUserCommand, User>();
        }

        private class Handler : UpdateCommandHandler<UpdateUserCommand, User>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task OnValidate(User entity, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrow(Command.Id);

                //TODO: add more generic approach
                if (Command.Role.HasFlag(Roles.Admin))
                    _drs.IsAdminElseThrow();

                if (Command.Role.HasFlag(Roles.Editor))
                    _drs.HasRole(Roles.Editor);

                if (Command.Role.HasFlag(Roles.User))
                    _drs.HasRole(Roles.User);

                await new UpdateUserValidator().ValidateAndThrowAsync(Command, cancellationToken: cancellationToken);

                if (Command.Email != entity.Email && await Uow.Users.IsEmailInUseAsync(Command.Email))
                {
                    throw new ValidationFailedException(nameof(Command.Email), ValidationMessages.Email.IsInUse);
                }
            }
        }
    }
}
