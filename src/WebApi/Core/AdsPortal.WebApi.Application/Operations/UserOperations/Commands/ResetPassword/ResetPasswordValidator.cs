namespace AdsPortal.Application.Operations.UserOperations.Commands.ResetPassword
{
    using Application.Constants;
    using FluentValidation;

    public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Token).NotEmpty()
                                 .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
