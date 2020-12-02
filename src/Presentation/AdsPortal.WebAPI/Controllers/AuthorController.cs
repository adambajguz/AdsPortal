namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AuthorOperations.Commands.CreateAuthor;
    using AdsPortal.Application.Operations.AuthorOperations.Commands.DeleteAuthor;
    using AdsPortal.Application.Operations.AuthorOperations.Commands.ImportAuthors;
    using AdsPortal.Application.Operations.AuthorOperations.Commands.UpdateAuthor;
    using AdsPortal.Application.Operations.AuthorOperations.Queries.GetAuthorDetails;
    using AdsPortal.Application.Operations.AuthorOperations.Queries.GetAuthorsList;
    using AdsPortal.Application.Operations.AuthorOperations.Queries.GetPagedAuthorsList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Application.OperationsModels.Importer;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/author")]
    [SwaggerTag("Create, update, and get author")]
    public class AuthorController : BaseController
    {
        public const string Import = nameof(Import);
        public const string Create = nameof(CreateAuthor);
        public const string GetDetails = nameof(GetAuthorDetails);
        public const string Update = nameof(UpdateAuthor);
        public const string Delete = nameof(DeleteAuthor);
        public const string GetAll = nameof(GetAuthorsList);
        public const string GetPaged = nameof(GetPagedAuthorsList);

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("import")]
        [SwaggerOperation(
            Summary = "Import authors from csv file",
            Description = "Imports authors from csv file")]
        [SwaggerResponse(StatusCodes.Status200OK, "Authors imported", typeof(ImportAuthorsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ImportAuthors([FromForm] ImportAuthorsCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new author",
            Description = "Creates a new author")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get author details",
            Description = "Gets author details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetAuthorDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetAuthorDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetAuthorDetailsQuery(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated author details",
            Description = "Updates author details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete author",
            Description = "Deletes author")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteAuthor([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteAuthorCommand(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all authors",
            Description = "Gets a list of all authors")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetAuthorsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetAuthorsList()
        {
            return Ok(await Mediator.Send(new GetAuthorsListQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged authors",
            Description = "Gets a paged list of authors")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetAuthorsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPagedAuthorsList([FromQuery] GetPagedAuthorsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
