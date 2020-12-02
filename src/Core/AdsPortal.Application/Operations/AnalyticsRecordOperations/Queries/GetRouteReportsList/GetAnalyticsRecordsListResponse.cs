namespace AdsPortal.Application.Operations.AnalyticsRecordOperations.Queries.GetRouteReportsList
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

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
