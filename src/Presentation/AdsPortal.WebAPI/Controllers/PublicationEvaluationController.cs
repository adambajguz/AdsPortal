namespace AdsPortal.WebAPI.Controllers
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForAuthors;
    using AdsPortal.Application.Operations.PublicationEvaluationOperations.Queries.GetEvaluationReportForDepartments;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/publication-evaluation")]
    [SwaggerTag("Create, update, and get publication evaluation reports")]
    public class PublicationEvaluationController : BaseController
    {
        public const string GetReportForAuthors = nameof(GetEvaluationReportForAuthors);

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-report-for-authors")]
        [SwaggerOperation(
            Summary = "Get evaluation report for authors",
            Description = "Gets evaluation report for authors")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetEvaluationReportForAuthorsResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetEvaluationReportForAuthors()
        {
            return Ok(await Mediator.Send(new GetEvaluationReportForAuthorsQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-report-for-departments")]
        [SwaggerOperation(
            Summary = "Get evaluation report for departments",
            Description = "Gets evaluation report for departments wiht or without report for authors")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetEvaluationReportForDepartmentsResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetEvaluationReportForDepartments([FromQuery] GetEvaluationReportForDepartmentsQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
