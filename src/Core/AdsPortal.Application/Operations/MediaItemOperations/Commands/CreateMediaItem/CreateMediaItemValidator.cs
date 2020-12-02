namespace AdsPortal.Application.Operations.MediaItemOperations.Commands.CreateMediaItem
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateMediaItemValidator : AbstractValidator<CreateMediaItemCommand>
    {
        public CreateMediaItemValidator()
        {
            RuleFor(x => x.File).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            When(x => !string.IsNullOrWhiteSpace(x.NewFileName), () =>
                {
                    RuleFor(x => x.GenerateFileName).NotEqual(true)
                                                    .WithMessage(ValidationMessages.General.InvalidValue);
                });
        }
    }
}
