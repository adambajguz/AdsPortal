namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
{
    using System;
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

            RuleFor(x => x.CoverImageId).NotEmpty()
                                        .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.VisibleTo).GreaterThan(DateTime.Now.AddMinutes(10))
                                     .WithMessage("Advertisement must be visible for at least 10 minutes.");
        }
    }
}
