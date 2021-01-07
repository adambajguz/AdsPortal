namespace AdsPortal.Application.Operations.CategoryOperations.Commands.CreateCategory
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.Description).NotEmpty()
                                       .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
