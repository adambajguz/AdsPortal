﻿namespace AdsPortal.Application.Operations.AdvertisementOperations.Commands.UpdateAdvertisement
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdateAdvertisementrValidator : AbstractValidator<UpdateAdvertisementCommand>
    {
        public UpdateAdvertisementrValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Title).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Description).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
