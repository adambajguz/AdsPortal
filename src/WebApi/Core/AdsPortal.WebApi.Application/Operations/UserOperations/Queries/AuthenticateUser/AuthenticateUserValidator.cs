namespace AdsPortal.Application.Operations.UserOperations.Queries.AuthenticateUser
{
    using Application.Constants;
    using FluentValidation;

    public sealed class AuthenticateUserValidator : AbstractValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(ValidationMessages.Email.HasWrongFormat);

            RuleFor(x => x.Password).NotEmpty()
                                    .WithMessage(ValidationMessages.Password.IsEmpty);
            RuleFor(x => x.Password).MinimumLength(8)
                                    .WithMessage(string.Format(ValidationMessages.Password.IsTooShort, 8));
            RuleFor(x => x.Password).MaximumLength(128)
                                    .WithMessage(string.Format(ValidationMessages.Password.IsTooLong, 128));
        }
    }
}
