namespace AdsPortal.ManagementUI.Services.Data
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForDepartments;
    using MediatR;

    public class PublicationEvaluationService
    {
        private IMediator Mediator { get; }

        public PublicationEvaluationService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<GetEvaluationReportForDepartmentsResponse> GetEvaluationReportForDepartments(bool includeAuthorReports)
        {
            return await Mediator.Send(new GetEvaluationReportForDepartmentsQuery { IncludeAuthorReports = includeAuthorReports });
        }
    }
}
