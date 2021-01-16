namespace AdsPortal.WebApi.Infrastructure.Identity.UserManager
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(x => x).NotEmpty()
                           .WithMessage(ValidationMessages.Password.IsEmpty);

            RuleFor(x => x).MinimumLength(8)
                           .WithMessage(string.Format(ValidationMessages.Password.IsTooShort, 8));

            RuleFor(x => x).MaximumLength(128)
                           .WithMessage(string.Format(ValidationMessages.Password.IsTooLong, 128));
        }
    }
}
