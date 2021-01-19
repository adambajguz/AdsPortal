namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsList
{
    using AdsPortal.WebApi.Application.Constants;
    using FluentValidation;

    public sealed class GetEntityAuditLogsByTableListQueryValidator : AbstractValidator<GetEntityAuditLogsByTableListQuery>
    {
        public GetEntityAuditLogsByTableListQueryValidator()
        {
            RuleFor(x => x.TableName).NotEmpty()
                                     .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}
