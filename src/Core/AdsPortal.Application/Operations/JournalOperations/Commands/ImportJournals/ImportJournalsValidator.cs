namespace AdsPortal.Application.Operations.JournalOperations.Commands.ImportJournals
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class ImportJournalsValidator : AbstractValidator<ImportJournalsCommand>
    {
        public ImportJournalsValidator()
        {
            RuleFor(x => x.File).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
