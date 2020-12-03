namespace AdsPortal.Application.Operations.UserOperations.Commands.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Constants;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Utils;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class CreateUserCommand : ICreateCommand
    {
        public string? Email { get; init; }
        public string? Password { get; init; }

        public string? Name { get; init; }
        public string? Surname { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Address { get; init; }

        public Roles Role { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateUserCommand, User>();
        }

        private class Handler : CreateCommandHandler<CreateUserCommand, User>
        {
            private readonly IUserManagerService _userManager;
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IUserManagerService userManager, IDataRightsService drs) : base(uow, mapper)
            {
                _userManager = userManager;
                _drs = drs;
            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task OnValidate(CancellationToken cancellationToken)
            {
                //TODO: add more generic approach
                if (Command.Role.HasFlag(Roles.Admin))
                    _drs.IsAdminElseThrow();

                if (Command.Role.HasFlag(Roles.User))
                    _drs.HasRole(Roles.User);

                await ValidationUtils.ValidateAndThrowAsync<CreateUserValidator, CreateUserCommand>(Command, cancellationToken);

                if (await Uow.Users.IsEmailInUseAsync(Command.Email))
                {
                    throw new ValidationFailedException(nameof(Command.Email), ValidationMessages.Email.IsInUse);
                }
            }

            protected override async Task OnMapped(User entity, CancellationToken cancellationToken)
            {
                entity.IsActive = true;
                await _userManager.SetPassword(entity, Command.Password ?? string.Empty, cancellationToken);
            }
        }
    }
}
