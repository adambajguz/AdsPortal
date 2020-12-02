namespace AdsPortal.Application.Operations.AuthenticationOperations.Commands.ResetPassword
{
    using Application.Constants;
    using FluentValidation;

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token).NotEmpty()
                                 .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
