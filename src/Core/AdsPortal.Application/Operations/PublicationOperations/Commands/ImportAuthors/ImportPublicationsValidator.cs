namespace AdsPortal.Application.Operations.AuthorOperations.Commands.ImportAuthors
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class ImportPublicationsValidator : AbstractValidator<ImportPublicationsCommand>
    {
        public ImportPublicationsValidator()
        {
            RuleFor(x => x.File).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
