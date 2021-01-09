namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.SendResetPasswordToken
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class SendResetPasswordTokenValidator : AbstractValidator<SendResetPasswordTokenQuery>
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
