namespace AdsPortal.Application.Operations.JournalOperations.Commands.CreateJournal
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateJournalValidator : AbstractValidator<CreateJournalCommand>
    {
        public CreateJournalValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ISSN).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.EISSN).NotEmpty()
                                 .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Disciplines).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Points).GreaterThan(0)
                                  .WithMessage(ValidationMessages.General.GreaterThenZero);
        }
    }
}
