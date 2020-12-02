namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.DegreeOperations.Commands.CreateDegree;
    using AdsPortal.Application.Operations.DegreeOperations.Commands.DeleteDegree;
    using AdsPortal.Application.Operations.DegreeOperations.Commands.UpdateDegree;
    using AdsPortal.Application.Operations.DegreeOperations.Queries.GetDegreeDetails;
    using AdsPortal.Application.Operations.DegreeOperations.Queries.GetDegreesList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/degree")]
    [SwaggerTag("Create, update, and get degree")]
    public class DegreeController : BaseController
    {
        public const string Create = nameof(CreateDegree);
        public const string GetDetails = nameof(GetDegreeDetails);
        public const string Update = nameof(UpdateDegree);
        public const string Delete = nameof(DeleteDegree);
        public const string GetAll = nameof(GetDegreesList);

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new degree",
            Description = "Creates a new degree")]
        [SwaggerResponse(StatusCodes.Status200OK, "Degree created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateDegree([FromBody] CreateDegreeCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get degree details",
            Description = "Gets degree details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetDegreeDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetDegreeDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetDegreeDetailsQuery(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated degree details",
            Description = "Updates degree details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Degree details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateDegree([FromBody] UpdateDegreeCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete degree",
            Description = "Deletes degree")]
        [SwaggerResponse(StatusCodes.Status200OK, "Degree deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteDegree([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteDegreeCommand(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all degrees",
            Description = "Gets a list of all degrees")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetDegreesListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetDegreesList()
        {
            return Ok(await Mediator.Send(new GetDegreesListQuery()));
        }
    }
}
