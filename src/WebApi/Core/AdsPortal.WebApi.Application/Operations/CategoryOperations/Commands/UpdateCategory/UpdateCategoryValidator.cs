namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Commands.UpdateCategory
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Description).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
