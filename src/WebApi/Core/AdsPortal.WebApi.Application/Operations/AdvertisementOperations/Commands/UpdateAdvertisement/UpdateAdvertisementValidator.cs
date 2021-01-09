namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.UpdateAdvertisement
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class UpdateAdvertisementValidator : AbstractValidator<UpdateAdvertisementCommand>
    {
        public UpdateAdvertisementValidator()
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
