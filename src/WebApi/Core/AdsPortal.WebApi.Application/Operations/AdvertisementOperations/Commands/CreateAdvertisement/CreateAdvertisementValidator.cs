namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class CreateAdvertisementValidator : AbstractValidator<CreateAdvertisementCommand>
    {
        public CreateAdvertisementValidator()
        {
            RuleFor(x => x.Title).NotEmpty()
                                 .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Description).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
