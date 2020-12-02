namespace AdsPortal.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateAdvertisementValidator : AbstractValidator<CreateAdvertisementCommand>
    {
        public CreateAdvertisementValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Surname).NotEmpty()
                                   .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ORCID).NotNull()
                                 .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
