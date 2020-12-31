namespace AdsPortal.Application.Operations.UserOperations.Commands.ChangeUserPassword
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public class ChangeUserPasswordCommand : IOperation
    {
        public Guid UserId { get; init; }
        public string? OldPassword { get; init; }
        public string? NewPassword { get; init; }

        private class Handler : IRequestHandler<ChangeUserPasswordCommand>
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

                User user = await _uow.Users.SingleByIdAsync(command.UserId);

                ChangeUserPasswordCommandValidator.Model validationModel = new ChangeUserPasswordCommandValidator.Model(command, user);
                await new ChangeUserPasswordCommandValidator(_userManager).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _userManager.SetPassword(user, command.NewPassword ?? string.Empty, cancellationToken);
                _uow.Users.Update(user);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
