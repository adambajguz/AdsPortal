namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.ChangeUserPassword
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.NewPassword).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
