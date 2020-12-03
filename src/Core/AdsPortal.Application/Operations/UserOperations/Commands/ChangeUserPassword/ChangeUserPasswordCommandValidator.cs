namespace AdsPortal.Application.Operations.UserOperations.Commands.ChangeUserPassword
{
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Domain.Entities;
    using Application.Constants;
    using FluentValidation;

    //TODO: remove (this should not be in validator as is complex)
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommandValidator.Model>
    {
        public ChangeUserPasswordCommandValidator(IUserManagerService _userManager)
        {
            RuleFor(x => x.Data.OldPassword).MustAsync(async (request, val, token) => await _userManager.ValidatePassword(request.User, val ?? string.Empty))
                                            .WithMessage(ValidationMessages.Password.OldIsIncorrect);
        }

        public class Model
        {
            public ChangeUserPasswordCommand Data { get; init; }
            public User User { get; init; }

            public Model(ChangeUserPasswordCommand data, User user)
            {
                Data = data;
                User = user;
            }
        }
    }
}
