namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Commands.CreateMediaItem
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class CreateMediaItemValidator : AbstractValidator<CreateMediaItemCommand>
    {
        public CreateMediaItemValidator()
        {
            RuleFor(x => x.File).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.VirtualDirectory).NotEmpty()
                                            .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            When(x => !string.IsNullOrWhiteSpace(x.NewFileName), () =>
                {
                    RuleFor(x => x.GenerateFileName).NotEqual(true)
                                                    .WithMessage(ValidationMessages.General.InvalidValue);
                });
        }
    }
}
