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
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public Roles Role { get; set; }

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

                if (Command.Role.HasFlag(Roles.Editor))
                    _drs.HasRole(Roles.Editor);

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
