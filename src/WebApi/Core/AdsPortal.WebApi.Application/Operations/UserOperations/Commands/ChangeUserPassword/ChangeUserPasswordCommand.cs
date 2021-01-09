namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.ChangeUserPassword
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Constants;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public sealed record ChangeUserPasswordCommand : IOperation
    {
        public Guid UserId { get; init; }
        public string? OldPassword { get; init; }
        public string? NewPassword { get; init; }

        private sealed class Handler : IRequestHandler<ChangeUserPasswordCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IDataRightsService _drs;
            private readonly IUserManagerService _userManager;

            public Handler(IAppRelationalUnitOfWork uow, IDataRightsService drs, IUserManagerService userManager)
            {
                _uow = uow;
                _drs = drs;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(ChangeUserPasswordCommand command, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrow(command.UserId);
                await new ChangeUserPasswordCommandValidator().ValidateAndThrowAsync(command, cancellationToken: cancellationToken);

                User user = await _uow.Users.SingleByIdAsync(command.UserId, cancellationToken: cancellationToken);

                if (!await _userManager.ValidatePassword(user, command.OldPassword ?? string.Empty, cancellationToken))
                    throw new ValidationFailedException(nameof(command.OldPassword), ValidationMessages.Password.OldIsIncorrect);

                await _userManager.SetPassword(user, command.NewPassword ?? string.Empty, cancellationToken);
                _uow.Users.Update(user);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
