namespace AdsPortal.Application.Operations.AnalyticsRecordOperations.Queries.GetRouteReportsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using MediatR;

    public class GetAnalyticsRecordsListQuery : IOperation<GetAnalyticsRecordsListResponse>
    {
        public GetAnalyticsRecordsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetAnalyticsRecordsListQuery, GetAnalyticsRecordsListResponse>
        {
            private readonly IMongoUnitOfWork _uow;

            public Handler(IMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetAnalyticsRecordsListResponse> Handle(GetAnalyticsRecordsListQuery query, CancellationToken cancellationToken)
            {
                return new GetAnalyticsRecordsListResponse
                {
                    AnalyticsRecords = await _uow.AnalyticsRecords.ProjectToAsync<GetAnalyticsRecordsListResponse.AnalyticsRecordLookupModel>()
                };
            }
        }
    }
}

