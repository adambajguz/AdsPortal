namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class CleanupEntityAuditLogByKeyValidator : AbstractValidator<CleanupEntityAuditLogByKeyCommand>
    {
        public CleanupEntityAuditLogByKeyValidator()
        {
            RuleFor(x => x.Key).NotEmpty()
                               .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
