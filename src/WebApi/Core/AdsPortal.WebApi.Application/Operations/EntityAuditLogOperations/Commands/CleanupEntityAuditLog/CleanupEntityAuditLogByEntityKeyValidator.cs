namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class CleanupEntityAuditLogByEntityKeyValidator : AbstractValidator<CleanupEntityAuditLogByEntityKeyCommand>
    {
        public CleanupEntityAuditLogByEntityKeyValidator()
        {
            RuleFor(x => x.Key).NotEmpty()
                               .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
