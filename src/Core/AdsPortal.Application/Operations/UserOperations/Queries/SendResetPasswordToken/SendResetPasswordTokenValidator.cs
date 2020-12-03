namespace AdsPortal.Application.Operations.UserOperations.Queries.SendResetPasswordToken
{
    using Application.Constants;
    using FluentValidation;

    public class SendResetPasswordTokenValidator : AbstractValidator<SendResetPasswordTokenQuery>
    {
        public SendResetPasswordTokenValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(ValidationMessages.Email.HasWrongFormat);
        }
    }
}
