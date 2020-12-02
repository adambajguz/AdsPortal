namespace AdsPortal.Application.Operations.DegreeOperations.Commands.UpdateDegree
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdateDegreeValidator : AbstractValidator<UpdateDegreeCommand>
    {
        public UpdateDegreeValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Name).NotNull()
                                .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
