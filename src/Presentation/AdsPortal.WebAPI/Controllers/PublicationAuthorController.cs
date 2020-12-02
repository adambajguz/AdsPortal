namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.CreatePublicationAuthor;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.DeletePublicationAuthor;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.UpdatePublicationAuthor;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPagedPublicationAuthorsList;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorDetails;
    using AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorsList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/publication-author")]
    [SwaggerTag("Create, update, and get publication author")]
    public class PublicationAuthorController : BaseController
    {
        public const string Create = nameof(CreatePublicationAuthor);
        public const string GetDetails = nameof(GetPublicationAuthorDetails);
        public const string Update = nameof(UpdatePublicationAuthor);
        public const string Delete = nameof(DeletePublicationAuthor);
        public const string GetAll = nameof(GetPublicationAuthorsList);
        public const string GetPaged = nameof(GetPagedPublicationAuthorsList);

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new publication author",
            Description = "Creates a new publication author")]
        [SwaggerResponse(StatusCodes.Status200OK, "PublicationAuthor created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreatePublicationAuthor([FromBody] CreatePublicationAuthorCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get publication author details",
            Description = "Gets publication author details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetPublicationAuthorDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPublicationAuthorDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetPublicationAuthorDetailsQuery(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated publication author details",
            Description = "Updates publication author details")]
        [SwaggerResponse(StatusCodes.Status200OK, "PublicationAuthor details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdatePublicationAuthor([FromBody] UpdatePublicationAuthorCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete publication author",
            Description = "Deletes publication author")]
        [SwaggerResponse(StatusCodes.Status200OK, "PublicationAuthor deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeletePublicationAuthor([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeletePublicationAuthorCommand(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all publication authors",
            Description = "Gets a list of all publication authors")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetPublicationAuthorsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPublicationAuthorsList()
        {
            return Ok(await Mediator.Send(new GetPublicationAuthorsListQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged publication authors",
            Description = "Gets a paged list of publication authors")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetPublicationAuthorsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPagedPublicationAuthorsList([FromQuery] GetPagedPublicationAuthorsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
