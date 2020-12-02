﻿namespace AdsPortal.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateAdvertisementValidator : AbstractValidator<CreateAdvertisementCommand>
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
