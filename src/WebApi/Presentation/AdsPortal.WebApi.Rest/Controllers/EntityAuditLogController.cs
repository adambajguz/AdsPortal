namespace AdsPortal.WebApi.Rest.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog;
    using AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.RevertUsingEntityAuditLog;
    using AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogDetails;
    using AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsForEntityList;
    using AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsList;
    using AdsPortal.WebApi.Attributes;
    using AdsPortal.WebApi.Domain.Jwt;
    using MediatR.GenericOperations.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/entity-audit-log")]
    [SwaggerTag("Revert entity and get entity audit log")]
    public sealed class EntityAuditLogController : BaseApiController
    {
        [CustomAuthorize(Roles.Admin)]
        [HttpPost("revert")]
        [SwaggerOperation(
            Summary = "Revert entity using audit log",
            Description = "Revert entity using entity audit log")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> RevertEntityUsingAuditLog([FromBody] RevertUsingEntityAuditLogCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get audit log details",
            Description = "Gets audit log details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetEntityAuditLogDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetAuditLogDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetEntityAuditLogDetailsQuery { Id = id }));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpDelete("cleanup")]
        [SwaggerOperation(
            Summary = "Delete audit log",
            Description = "Deletes route log")]
        [SwaggerResponse(StatusCodes.Status200OK, "Route log deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CleanupAuditLog([FromBody] CleanupEntityAuditLogCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all-for-entity/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get all route logs for entity",
            Description = "Gets a list of all entity audit logs for specified entity")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetEntityAuditLogsForEntityListResponse>))]
        public async Task<IActionResult> GetEntityAuditLogsForEntityList([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetEntityAuditLogsForEntityListQuery { Id = id }));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all route logs",
            Description = "Gets a list of all entity audit logs")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetEntityAuditLogsListResponse>))]
        public async Task<IActionResult> GetEntityAuditLogsList()
        {
            return Ok(await Mediator.Send(new GetEntityAuditLogsListQuery()));
        }
    }
}
