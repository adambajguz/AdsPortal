namespace AdsPortal.Application.Operations.UserOperations.Commands.ResetPassword
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using Domain.Jwt;
    using FluentValidation;
    using MediatR;

    public class ResetPasswordCommand : IOperation
    {
        public string? Token { get; init; }
        public string? Password { get; init; }

        private class Handler : IRequestHandler<ResetPasswordCommand>
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
                await _uow.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}

