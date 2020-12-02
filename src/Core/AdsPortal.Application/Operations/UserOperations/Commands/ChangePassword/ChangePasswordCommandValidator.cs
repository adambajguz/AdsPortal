namespace AdsPortal.Application.Operations.UserOperations.Commands.ChangePassword
{
    using Application.Constants;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Domain.Entities;
    using FluentValidation;

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommandValidator.Model>
    {
        public ChangePasswordCommandValidator(IUserManagerService _userManager)
        {
            RuleFor(x => x.Data.OldPassword).MustAsync(async (request, val, token) => await _userManager.ValidatePassword(request.User, val ?? string.Empty))
                                            .WithMessage(ValidationMessages.Password.OldIsIncorrect);
        }

        public class Model
        {
            public ChangePasswordCommand Data { get; set; }
            public User User { get; set; }

            public Model(ChangePasswordCommand data, User user)
            {
                Data = data;
                User = user;
            }
        }
    }
}
