namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogDetails
{
    using System;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogDetailsQuery : IGetDetailsQuery<GetEntityAuditLogDetailsResponse>, IIdentifiableOperation<GetEntityAuditLogDetailsResponse>
    {
        public Guid Id { get; init; }

        private class Handler : GetDetailsByIdQueryHandler<GetEntityAuditLogDetailsQuery, EntityAuditLog, GetEntityAuditLogDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }
        }
    }
}
