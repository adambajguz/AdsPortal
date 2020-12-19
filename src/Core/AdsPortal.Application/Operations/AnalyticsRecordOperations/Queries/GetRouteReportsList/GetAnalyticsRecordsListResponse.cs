namespace AdsPortal.Application.Operations.AnalyticsRecordOperations.Queries.GetRouteReportsList
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetAnalyticsRecordsListResponse : IOperationResult
    {
        public IList<AnalyticsRecordLookupModel> AnalyticsRecords { get; init; } = new List<AnalyticsRecordLookupModel>();

        public class AnalyticsRecordLookupModel : IIdentifiableOperationResult, ICustomMapping
        {
            public Guid Id { get; init; }

            public DateTime Timestamp { get; init; }
            public string Uri { get; init; } = default!;
            public ulong Visits { get; init; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<AnalyticsRecord, AnalyticsRecordLookupModel>();
            }
        }
    }
}
