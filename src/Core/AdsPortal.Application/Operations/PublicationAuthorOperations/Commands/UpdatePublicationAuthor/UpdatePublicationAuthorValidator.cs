namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.UpdatePublicationAuthor
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdatePublicationAuthorValidator : AbstractValidator<UpdatePublicationAuthorCommand>
    {
        public UpdatePublicationAuthorValidator()
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
