namespace AdsPortal.Application.Operations.AuthorOperations.Commands.UpdateAuthor
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Surname).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ORCID).NotNull()
                                 .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
