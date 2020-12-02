namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Abstractions.Enums;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetEntityAuditLogsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public string TableName { get; set; } = default!;
        public Guid Key { get; set; } = default!;
        public AuditActions Action { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<EntityAuditLog, GetEntityAuditLogsListResponse>();
        }
    }
}
