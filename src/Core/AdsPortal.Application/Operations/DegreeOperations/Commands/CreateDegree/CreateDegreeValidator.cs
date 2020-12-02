namespace AdsPortal.Application.Operations.DegreeOperations.Commands.CreateDegree
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateDegreeValidator : AbstractValidator<CreateDegreeCommand>
    {
        public CreateDegreeValidator()
        {
            RuleFor(x => x.Name).NotNull()
                                .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
