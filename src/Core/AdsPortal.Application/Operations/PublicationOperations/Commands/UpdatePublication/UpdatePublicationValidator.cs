namespace AdsPortal.Application.Operations.PublicationOperations.Commands.UpdatePublication
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdatePublicationValidator : AbstractValidator<UpdatePublicationCommand>
    {
        public UpdatePublicationValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Surname).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ORCID).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
