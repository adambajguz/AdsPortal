namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.CreatePublicationAuthor
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreatePublicationAuthorValidator : AbstractValidator<CreatePublicationAuthorCommand>
    {
        public CreatePublicationAuthorValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Surname).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ORCID).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
