﻿namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System;
    using AdsPortal.WebApi.Domain.Abstractions.Enums;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetEntityAuditLogsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }
        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }

        public string TableName { get; init; } = default!;
        public Guid Key { get; init; } = default!;
        public AuditActions Action { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<EntityAuditLog, GetEntityAuditLogsListResponse>();
        }
    }
}