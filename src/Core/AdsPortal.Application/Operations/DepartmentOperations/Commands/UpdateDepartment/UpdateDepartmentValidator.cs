namespace AdsPortal.Application.Operations.DepartmentOperations.Commands.UpdateDepartment
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Name).NotNull()
                                .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
