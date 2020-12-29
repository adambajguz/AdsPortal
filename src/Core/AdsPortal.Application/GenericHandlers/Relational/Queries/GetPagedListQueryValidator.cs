namespace AdsPortal.Application.GenericHandlers.Relational.Queries
{
    using AdsPortal.Application.Constants;
    using FluentValidation;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetPagedListQueryValidator<TResultEntry> : AbstractValidator<IGetPagedListQuery<TResultEntry>>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public GetPagedListQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(0)
                                .WithMessage(ValidationMessages.General.GreaterOrEqualZero);

            RuleFor(x => x.EntiresPerPage).GreaterThan(2)
                                          .WithMessage(string.Format(ValidationMessages.General.GreaterThenN, 2));
        }
    }
}
