namespace AdsPortal.Application.Operations.AuthorOperations.Commands.ImportAuthors
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class ImportAuthorsValidator : AbstractValidator<ImportAuthorsCommand>
    {
        public ImportAuthorsValidator()
        {
            RuleFor(x => x.File).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
