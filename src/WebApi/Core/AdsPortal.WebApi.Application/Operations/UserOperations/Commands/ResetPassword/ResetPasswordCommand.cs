namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.ResetPassword
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public sealed record ResetPasswordCommand : IOperation
    {
        public string? Token { get; init; }
        public string? Password { get; init; }

        private sealed class Handler : IRequestHandler<ResetPasswordCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IJwtService _jwt;
            private readonly IUserManagerService _userManager;

            public Handler(IAppRelationalUnitOfWork uow, IJwtService jwt, IUserManagerService userManager)
            {
                _uow = uow;
                _jwt = jwt;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
            {
                await new ResetPasswordValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

                if (!_jwt.IsTokenStringValid(command.Token) || !_jwt.IsRoleInToken(command.Token, Roles.ResetPassword))
                    throw new ForbiddenException();

                Guid userId = _jwt.GetUserIdFromToken(command.Token!);
                User user = await _uow.Users.FirstAsync(x => x.Id.Equals(userId), cancellationToken: cancellationToken);

                await _userManager.SetPassword(user, command.Password ?? string.Empty, cancellationToken);

                _uow.Users.Update(user);
                await _uow.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}

