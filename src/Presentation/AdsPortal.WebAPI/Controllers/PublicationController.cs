namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AuthorOperations.Commands.ImportAuthors;
    using AdsPortal.Application.Operations.PublicationOperations.Commands.CreatePublication;
    using AdsPortal.Application.Operations.PublicationOperations.Commands.DeletePublication;
    using AdsPortal.Application.Operations.PublicationOperations.Commands.UpdatePublication;
    using AdsPortal.Application.Operations.PublicationOperations.Queries.GetPagedPublicationsList;
    using AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationDetails;
    using AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationsList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Application.OperationsModels.Importer;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/publication")]
    [SwaggerTag("Create, update, and get publication")]
    public class PublicationController : BaseController
    {
        public const string Import = nameof(ImportPublications);
        public const string Create = nameof(CreatePublication);
        public const string GetDetails = nameof(GetPublicationDetails);
        public const string Update = nameof(UpdatePublication);
        public const string Delete = nameof(DeletePublication);
        public const string GetAll = nameof(GetPublicationsList);
        public const string GetPaged = nameof(GetPagedPublicationsList);

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("import")]
        [SwaggerOperation(
            Summary = "Import publications from csv file",
            Description = "Imports publications from csv file")]
        [SwaggerResponse(StatusCodes.Status200OK, "Publications imported", typeof(ImportPublicationsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ImportPublications([FromForm] ImportPublicationsCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new publication",
            Description = "Creates a new publication")]
        [SwaggerResponse(StatusCodes.Status200OK, "Publication created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreatePublication([FromBody] CreatePublicationCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get publication details",
            Description = "Gets publication details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetPublicationDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPublicationDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetPublicationDetailsQuery(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated publication details",
            Description = "Updates publication details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Publication details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdatePublication([FromBody] UpdatePublicationCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete publication",
            Description = "Deletes publication")]
        [SwaggerResponse(StatusCodes.Status200OK, "Publication deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeletePublication([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeletePublicationCommand(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all publications",
            Description = "Gets a list of all publications")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetPublicationsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPublicationsList()
        {
            return Ok(await Mediator.Send(new GetPublicationsListQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged publications",
            Description = "Gets a paged list of publications")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetPublicationsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPagedPublicationsList([FromQuery] GetPagedPublicationsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
