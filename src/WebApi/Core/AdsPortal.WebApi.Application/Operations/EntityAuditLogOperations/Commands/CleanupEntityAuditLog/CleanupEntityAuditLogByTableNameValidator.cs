namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class CleanupEntityAuditLogByTableNameValidator : AbstractValidator<CleanupEntityAuditLogByTableNameCommand>
    {
        public CleanupEntityAuditLogByTableNameValidator()
        {
            RuleFor(x => x.TableName).NotEmpty()
                                     .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
