namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsList
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class GetEntityAuditLogsByEntityKeyListQueryValidator : AbstractValidator<GetEntityAuditLogsByEntityKeyListQuery>
    {
        public GetEntityAuditLogsByEntityKeyListQueryValidator()
        {
            RuleFor(x => x.Key).NotEmpty()
                               .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
