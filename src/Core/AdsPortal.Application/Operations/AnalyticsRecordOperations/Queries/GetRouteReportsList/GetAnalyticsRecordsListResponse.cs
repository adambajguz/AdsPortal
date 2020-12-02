namespace AdsPortal.Application.Operations.AnalyticsRecordOperations.Queries.GetRouteReportsList
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetAnalyticsRecordsListResponse : IOperationResult
    {
        public IList<AnalyticsRecordLookupModel> AnalyticsRecords { get; set; } = default!;

        public class AnalyticsRecordLookupModel : IIdentifiableOperationResult, ICustomMapping
        {
            public Guid Id { get; set; }

            public DateTime Timestamp { get; set; }
            public string Uri { get; set; } = default!;
            public ulong Visits { get; set; }

            void ICustomMapping.CreateMappings(Profile configuration)
            {
                configuration.CreateMap<AnalyticsRecord, AnalyticsRecordLookupModel>();
            }
        }
    }
}
