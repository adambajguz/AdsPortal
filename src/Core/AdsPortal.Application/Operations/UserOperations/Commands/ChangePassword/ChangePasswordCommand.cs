namespace AdsPortal.Application.Operations.UserOperations.Commands.ChangePassword
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using FluentValidation;
    using MediatR;

    public class ChangePasswordCommand : IOperation
    {
        public Guid UserId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }

        private class Handler : IRequestHandler<ChangePasswordCommand>
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

            public async Task<Unit> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrow(command.UserId);

                User user = await _uow.Users.SingleByIdAsync(command.UserId);

                ChangePasswordCommandValidator.Model validationModel = new ChangePasswordCommandValidator.Model(command, user);
                await new ChangePasswordCommandValidator(_userManager).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _userManager.SetPassword(user, command.NewPassword ?? string.Empty, cancellationToken);
                _uow.Users.Update(user);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
