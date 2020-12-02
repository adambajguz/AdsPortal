namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.DepartmentOperations.Commands.CreateDepartment;
    using AdsPortal.Application.Operations.DepartmentOperations.Commands.DeleteDepartment;
    using AdsPortal.Application.Operations.DepartmentOperations.Commands.UpdateDepartment;
    using AdsPortal.Application.Operations.DepartmentOperations.Queries.GetDepartmentDetails;
    using AdsPortal.Application.Operations.DepartmentOperations.Queries.GetDepartmentsList;
    using AdsPortal.Application.Operations.DepartmentOperations.Queries.GetPagedDepartmentsList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/department")]
    [SwaggerTag("Create, update, and get department")]
    public class DepartmentController : BaseController
    {
        public const string Create = nameof(CreateDepartment);
        public const string GetDetails = nameof(GetDepartmentDetails);
        public const string Update = nameof(UpdateDepartment);
        public const string Delete = nameof(DeleteDepartment);
        public const string GetAll = nameof(GetDepartmentsList);
        public const string GetPaged = nameof(GetPagedDepartmentsList);

        [CustomAuthorize(Roles.Admin)]
        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create new department",
            Description = "Creates a new department")]
        [SwaggerResponse(StatusCodes.Status200OK, "Department created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get department details",
            Description = "Gets department details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetDepartmentDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetDepartmentDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetDepartmentDetailsQuery(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Updated department details",
            Description = "Updates department details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Department details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete department",
            Description = "Deletes department")]
        [SwaggerResponse(StatusCodes.Status200OK, "Department deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteDepartmentCommand(id)));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all departments",
            Description = "Gets a list of all departments")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetDepartmentsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetDepartmentsList()
        {
            return Ok(await Mediator.Send(new GetDepartmentsListQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-paged")]
        [SwaggerOperation(
            Summary = "Get paged departments",
            Description = "Gets a paged list of departments")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetDepartmentsListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetPagedDepartmentsList([FromQuery] GetPagedDepartmentsListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
