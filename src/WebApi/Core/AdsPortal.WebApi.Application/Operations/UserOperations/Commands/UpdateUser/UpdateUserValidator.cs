namespace AdsPortal.Application.Operations.UserOperations.Commands.UpdateUser
{
    using AdsPortal.WebApi.Domain.Jwt;
    using Application.Constants;
    using FluentValidation;

    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
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

            RuleFor(x => x.Address).NotNull()
                                   .WithMessage(ValidationMessages.General.IsNull);

            RuleFor(x => x.PhoneNumber).NotNull()
                                       .WithMessage(ValidationMessages.General.IsNull);

            RuleFor(x => x.Role).IsInEnum()
                                .NotEqual(Roles.ResetPassword)
                                .WithMessage(ValidationMessages.User.InvalidRole);
        }
    }
}
