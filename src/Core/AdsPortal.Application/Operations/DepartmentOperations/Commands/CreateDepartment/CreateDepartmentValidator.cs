namespace AdsPortal.Application.Operations.DepartmentOperations.Commands.CreateDepartment
{
    using AdsPortal.Application.Constants;
    using FluentValidation;

    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name).NotNull()
                                .WithMessage(ValidationMessages.General.IsNull);
        }
    }
}
