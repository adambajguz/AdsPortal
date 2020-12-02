namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogDetails
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Abstractions.Enums;
    using AdsPortal.Domain.Mapping;
    using Domain.Entities;

    public class GetEntityAuditLogDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public string TableName { get; set; } = default!;
        public Guid Key { get; set; } = default!;
        public AuditActions Action { get; set; }
        public string? Values { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<EntityAuditLog, GetEntityAuditLogDetailsResponse>();
        }
    }
}
