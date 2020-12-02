namespace AdsPortal.WebAPI.Controllers
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AnalyticsRecordOperations.Commands.DeleteOldAnalyticsRecords;
    using AdsPortal.Application.Operations.AnalyticsRecordOperations.Queries.GetRouteReportsList;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/analytics-record")]
    [SwaggerTag("Get analytics records")]
    public class AnalyticsRecordController : BaseController
    {
        public const string DeleteOld = nameof(DeleteOldAnalyticsRecords);
        public const string GetAll = nameof(GetAnalyticsRecordsList);

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete-old")]
        [SwaggerOperation(
            Summary = "Delete old analytics records",
            Description = "Deletes all analytics records with Timestamp lower or equal to specified date. If Date is null then deletes all.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route logs deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteOldAnalyticsRecords([FromBody] DeleteOldAnalyticsRecordsCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all analytics records",
            Description = "Gets a list of all analytics records")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetAnalyticsRecordsListQuery))]
        public async Task<IActionResult> GetAnalyticsRecordsList()
        {
            return Ok(await Mediator.Send(new GetAnalyticsRecordsListQuery()));
        }
    }
}
