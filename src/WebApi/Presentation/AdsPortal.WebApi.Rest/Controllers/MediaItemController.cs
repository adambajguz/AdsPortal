namespace AdsPortal.WebApi.Rest.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Commands.CreateMediaItem;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Commands.DeleteMediaItem;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemChecksumFile;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList;
    using AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemsStatistics;
    using AdsPortal.WebApi.Attributes;
    using AdsPortal.WebApi.Domain.Jwt;
    using MediatR.GenericOperations.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/media/")]
    [SwaggerTag("Get media")]
    public sealed class MediaItemController : BaseController
    {
        public const string Create = nameof(CreateMedia);
        public const string GetFileByPath = nameof(GetMediaFileByPath);
        public const string GetFileById = nameof(GetMediaFileById);
        public const string GetDetailsByPath = nameof(GetMediaDetailsByPath);
        public const string GetDetailsById = nameof(GetMediaDetailsById);
        public const string GetChecksumByPath = nameof(GetMediaChecksumByPath);
        public const string GetChecksumById = nameof(GetMediaChecksumById);
        public const string Delete = nameof(DeleteMedia);
        public const string GetAll = nameof(GetMediaList);
        public const string GetStatistics = nameof(GetMediaStatistics);
        public const string GetPaged = nameof(GetPagedMediaList);

        [CustomAuthorize(Roles.User)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create a new media item",
            Description = "Creates a new media item")]
        [SwaggerResponse(StatusCodes.Status200OK, "Media item created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateMedia([FromForm] CreateMediaItemCommand request)
        {
            return base.Ok(await Mediator.Send(request));
        }

        [HttpGet("file/{**mediaPath}")]
        [SwaggerOperation(
            Summary = "Get media file by path",
            Description = "Gets media file by path")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(FileContentResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaFileByPath([FromRoute] string mediaPath, [FromQuery] bool download = false)
        {
            string decodedMediaPath = HttpUtility.UrlDecode(mediaPath);
            GetMediaItemFileResponse response = await Mediator.Send(new GetMediaItemFileByPathCommand { Path = decodedMediaPath });

            if (download)
            {
                return File(response.Data ?? Array.Empty<byte>(), response.ContentType, response.FileName);
            }

            return File(response.Data ?? Array.Empty<byte>(), response.ContentType);
        }

        [HttpGet("file-by-id/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get media file",
            Description = "Gets media file")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(FileContentResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaFileById([FromRoute] Guid id, [FromQuery] bool download = false)
        {
            GetMediaItemFileResponse response = await Mediator.Send(new GetMediaItemFileByIdCommand { Id = id });

            if (download)
            {
                return File(response.Data ?? Array.Empty<byte>(), response.ContentType, response.FileName);
            }

            return File(response.Data ?? Array.Empty<byte>(), response.ContentType);
        }


        [HttpGet("get-by-path/{**mediaPath}")]
        [SwaggerOperation(
            Summary = "Get media item",
            Description = "Gets media item")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetMediaItemDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaDetailsByPath([FromRoute] string mediaPath)
        {
            string decodedMediaPath = HttpUtility.UrlDecode(mediaPath);

            return Ok(await Mediator.Send(new GetMediaItemDetailsByPathQuery { Path = decodedMediaPath }));
        }

        [HttpGet("get-by-id/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get media item",
            Description = "Gets media item")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetMediaItemDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaDetailsById([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetMediaItemDetailsByIdQuery { Id = id }));
        }

        [HttpGet("get-checksum-file/{**mediaPath}")]
        [SwaggerOperation(
           Summary = "Get media item checksum by path",
           Description = "Gets media item checksum by path")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(FileContentResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaChecksumByPath([FromRoute] string mediaPath, [FromQuery] bool download = false)
        {
            string decodedMediaPath = HttpUtility.UrlDecode(mediaPath);
            GetMediaItemChecksumResponse response = await Mediator.Send(new GetMediaItemChecksumFileByPathCommand { Path = decodedMediaPath });

            if (download)
            {
                return File(response.FileByteContent, response.ContentType, response.FileName);
            }

            return File(response.FileByteContent, response.ContentType);
        }

        [HttpGet("get-checksum-file-by-id/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get media item checksum by id",
            Description = "Gets media item checksum by id")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(FileContentResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaChecksumById([FromRoute] Guid id, [FromQuery] bool download = false)
        {
            GetMediaItemChecksumResponse response = await Mediator.Send(new GetMediaItemChecksumFileByIdCommand { Id = id });

            if (download)
            {
                return File(response.FileByteContent, response.ContentType, response.FileName);
            }

            return File(response.FileByteContent, response.ContentType);
        }

        [CustomAuthorize(Roles.User)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete media item",
            Description = "Deletes media item")]
        [SwaggerResponse(StatusCodes.Status200OK, "Media item deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Media item not found", typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteMedia([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteMediaItemCommand { Id = id }));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all media items",
            Description = "Gets a list of media items")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetMediaItemsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaList()
        {
            return Ok(await Mediator.Send(new GetMediaItemsListQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get all media items",
            Description = "Gets a paged list of media items")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetMediaItemsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPagedMediaList([FromQuery] GetPagedMediaItemsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("statistics")]
        [SwaggerOperation(
            Summary = "Get media items statistics",
            Description = "Get media items statistics")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetMediaItemsStatisticsResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetMediaStatistics()
        {
            return Ok(await Mediator.Send(new GetMediaItemsStatisticsQuery()));
        }
    }
}
