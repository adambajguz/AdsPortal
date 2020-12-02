namespace AdsPortal.Infrastructure.Identity.UserManager
{
    using Application.Constants;
    using Common;
    using FluentValidation;

    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(x => x).NotEmpty()
                           .WithMessage(ValidationMessages.Password.IsEmpty);
            RuleFor(x => x).MinimumLength(GlobalAppConfig.MIN_PASSWORD_LENGTH)
                           .WithMessage(string.Format(ValidationMessages.Password.IsTooShort, GlobalAppConfig.MIN_PASSWORD_LENGTH));
        }
    }
}
