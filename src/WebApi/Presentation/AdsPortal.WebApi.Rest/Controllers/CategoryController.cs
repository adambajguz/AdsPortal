namespace AdsPortal.WebApi.Rest.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Operations.CategoryOperations.Commands.CreateCategory;
    using AdsPortal.WebApi.Application.Operations.CategoryOperations.Commands.DeleteCategory;
    using AdsPortal.WebApi.Application.Operations.CategoryOperations.Commands.UpdateCategory;
    using AdsPortal.WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList;
    using AdsPortal.WebApi.Application.Operations.CategoryOperations.Queries.GetCategoryDetails;
    using MediatR.GenericOperations.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/category")]
    [SwaggerTag("Create, update, and get category")]
    public sealed class CategoryController : BaseController
    {
        public const string Create = nameof(CreateCategory);
        public const string GetDetails = nameof(GetCategoryDetails);
        public const string Update = nameof(UpdateCategory);
        public const string Delete = nameof(DeleteCategory);
        public const string GetAll = nameof(GetCategorysList);
        public const string GetPaged = nameof(GetPagedCategorysList);

        //[CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new category",
            Description = "Creates a new category")]
        [SwaggerResponse(StatusCodes.Status200OK, "Category created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("get/{id:guid}", Name = GetDetails)]
        [SwaggerOperation(
            Summary = "Get category details",
            Description = "Gets category details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetCategoryDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetCategoryDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetCategoryDetailsQuery { Id = id }));
        }

        //[CustomAuthorize(Roles.Admin)]
        [HttpPut("update/{id:guid}")]
        [SwaggerOperation(
            Summary = "Update category details",
            Description = "Updates category details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Category details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        //[CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete category",
            Description = "Deletes category")]
        [SwaggerResponse(StatusCodes.Status200OK, "Category deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryCommand { Id = id }));
        }

        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all categorys",
            Description = "Gets a list of all categorys")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetCategoriesListResponse>))]
        public async Task<IActionResult> GetCategorysList()
        {
            return Ok(await Mediator.Send(new GetCategoriesListQuery()));
        }

        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged categorys",
            Description = "Gets a paged list of categorys")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetCategoriesListResponse>))]
        public async Task<IActionResult> GetPagedCategorysList([FromQuery] GetPagedCategoriesListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
