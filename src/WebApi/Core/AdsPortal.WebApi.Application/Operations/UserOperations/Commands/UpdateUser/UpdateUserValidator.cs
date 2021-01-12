namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.UpdateUser
{
    using AdsPortal.WebApi.Application.Constants;
    using AdsPortal.WebApi.Domain.Jwt;
    using FluentValidation;

    public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Email).EmailAddress()
                                 .WithMessage(ValidationMessages.Email.HasWrongFormat);

            RuleFor(x => x.Name).NotNull()
                                .WithMessage(ValidationMessages.General.IsNull);

            RuleFor(x => x.Surname).NotNull()
                                   .WithMessage(ValidationMessages.General.IsNull);

            RuleFor(x => x.Role).IsInEnum()
                                .NotEqual(Roles.ResetPassword)
                                .WithMessage(ValidationMessages.User.InvalidRole);
        }
    }
}
