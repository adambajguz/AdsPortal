namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.RevertUsingEntityAuditLog
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class RollbackUsingEntityAuditLogValidator : AbstractValidator<RollbackUsingEntityAuditLogCommand>
    {
        public RollbackUsingEntityAuditLogValidator()
        {
            RuleFor(x => x.EntityKey).NotEmpty()
                                    .WithMessage(ValidationMessages.General.IsNullOrEmpty);

            RuleFor(x => x.ToId).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
