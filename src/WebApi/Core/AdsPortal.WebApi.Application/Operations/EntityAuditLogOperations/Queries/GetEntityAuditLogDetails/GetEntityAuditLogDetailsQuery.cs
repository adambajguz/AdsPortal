namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogDetails
{
    using System;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogDetailsQuery : IGetDetailsQuery<GetEntityAuditLogDetailsResponse>, IIdentifiableOperation<GetEntityAuditLogDetailsResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetDetailsByIdQueryHandler<GetEntityAuditLogDetailsQuery, EntityAuditLog, GetEntityAuditLogDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }
        }
    }
}
