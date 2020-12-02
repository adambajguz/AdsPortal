namespace AdsPortal.Application.Operations.JournalOperations.Commands.UpdateJournal
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdateJournalValidator : AbstractValidator<UpdateJournalCommand>
    {
        public UpdateJournalValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
