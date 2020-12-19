namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement;
    using AdsPortal.Application.Operations.AdvertisementOperations.Commands.DeleteAdvertisement;
    using AdsPortal.Application.Operations.AdvertisementOperations.Commands.UpdateAdvertisement;
    using AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementDetails;
    using AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList;
    using AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetPagedAdvertisementsList;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using MediatR.GenericOperations.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/advertisement")]
    [SwaggerTag("Create, update, and get advertisement")]
    public class AdvertisementController : BaseController
    {
        public const string Create = nameof(CreateAdvertisement);
        public const string GetDetails = nameof(GetAdvertisementDetails);
        public const string Update = nameof(UpdateAdvertisement);
        public const string Delete = nameof(DeleteAdvertisement);
        public const string GetAll = nameof(GetAdvertisementsList);
        public const string GetPaged = nameof(GetPagedAdvertisementsList);

        [CustomAuthorize(Roles.User)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new advertisement",
            Description = "Creates a new advertisement")]
        [SwaggerResponse(StatusCodes.Status200OK, "Advertisement created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateAdvertisement([FromBody] CreateAdvertisementCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get advertisement details",
            Description = "Gets advertisement details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetAdvertisementDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetAdvertisementDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetAdvertisementDetailsQuery { Id = id }));
        }

        [CustomAuthorize(Roles.User)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated advertisement details",
            Description = "Updates advertisement details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Advertisement details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateAdvertisement([FromBody] UpdateAdvertisementCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.User)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete advertisement",
            Description = "Deletes advertisement")]
        [SwaggerResponse(StatusCodes.Status200OK, "Advertisement deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteAdvertisement([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteAdvertisementCommand { Id = id }));
        }

        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all advertisements",
            Description = "Gets a list of all advertisements")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetAdvertisementsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetAdvertisementsList()
        {
            return Ok(await Mediator.Send(new GetAdvertisementsListQuery()));
        }

        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged advertisements",
            Description = "Gets a paged list of advertisements")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetAdvertisementsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPagedAdvertisementsList([FromQuery] GetPagedAdvertisementsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
