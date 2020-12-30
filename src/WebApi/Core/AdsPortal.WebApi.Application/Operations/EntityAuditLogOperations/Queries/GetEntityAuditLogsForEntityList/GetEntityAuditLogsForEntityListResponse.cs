namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System;
    using AdsPortal.Domain.Abstractions.Enums;
    using AdsPortal.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetEntityAuditLogsForEntityListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }
        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }

        public string TableName { get; init; } = default!;
        public AuditActions Action { get; init; }
        public string? Values { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<EntityAuditLog, GetEntityAuditLogsForEntityListResponse>();
        }
    }
}
