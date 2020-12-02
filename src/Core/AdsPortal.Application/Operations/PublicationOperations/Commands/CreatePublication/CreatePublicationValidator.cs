namespace AdsPortal.Application.Operations.PublicationOperations.Commands.CreatePublication
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreatePublicationValidator : AbstractValidator<CreatePublicationCommand>
    {
        public CreatePublicationValidator()
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
