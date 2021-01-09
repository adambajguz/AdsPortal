namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.ResetPassword
{
    using AdsPortal.WebApi.Application.Constants;
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
