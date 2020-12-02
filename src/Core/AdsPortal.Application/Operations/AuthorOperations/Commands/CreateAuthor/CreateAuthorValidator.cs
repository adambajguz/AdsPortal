namespace AdsPortal.Application.Operations.AuthorOperations.Commands.CreateAuthor
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateAuthorValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Surname).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ORCID).NotNull()
                                 .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
