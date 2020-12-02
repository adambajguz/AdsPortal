namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Operations.UserOperations.Commands.ChangePassword;
    using AdsPortal.Application.Operations.UserOperations.Commands.CreateUser;
    using AdsPortal.Application.Operations.UserOperations.Commands.DeleteUser;
    using AdsPortal.Application.Operations.UserOperations.Commands.PatchUser;
    using AdsPortal.Application.Operations.UserOperations.Commands.UpdateUser;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetPagedJournalsList;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUserDetails;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.WebAPI.Attributes;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/user")]
    [SwaggerTag("Create, update, and get user")]
    public class UserController : BaseController
    {
        public const string Create = nameof(CreateUser);
        public const string GetCurrentDetails = nameof(GetCurrentUserDetails);
        public const string GetDetails = nameof(GetUserDetails);
        public const string Update = nameof(UpdateUser);
        public const string Patch = nameof(PatchUser);
        public const string Delete = nameof(DeleteUser);
        public const string ChangePassword = nameof(ChangeUserPassword);
        public const string GetAll = nameof(GetUsersList);
        public const string GetPaged = nameof(GetPagedUsersList);

        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create (register) a new user",
            Description = "Creates a new user (requires Admin role to create admin account)")]
        [SwaggerResponse(StatusCodes.Status200OK, "User created", typeof(IdResult))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand request)
        {
            return base.Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.User)]
        [HttpGet("get-current")]
        [SwaggerOperation(
            Summary = "Get authenticated user details",
            Description = "Gets authenticated user details based on token")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetUserDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetCurrentUserDetails([FromServices] ICurrentUserService currentUser)
        {
            Guid userId = currentUser.UserId ?? throw new ForbiddenException();

            return Ok(await Mediator.Send(new GetUserDetailsQuery { Id = userId }));
        }

        [CustomAuthorize(Roles.User)]
        [HttpGet("get/{id:guid}")]
        [SwaggerOperation(
            Summary = "Get user details",
            Description = "Gets user details")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(GetUserDetailsResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetUserDetails([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetUserDetailsQuery { Id = id }));
        }

        [CustomAuthorize(Roles.User)]
        [HttpPut("update")]
        [SwaggerOperation(
            Summary = "Update user details",
            Description = "Updates user details (requires Admin role to update to admin account)")]
        [SwaggerResponse(StatusCodes.Status200OK, "User details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.User)]
        [HttpPatch("patch")]
        [SwaggerOperation(
            Summary = "Patch user details",
            Description = "Patch user details (requires Admin role to update to admin account)")]
        [SwaggerResponse(StatusCodes.Status200OK, "User details updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> PatchUser([FromBody] PatchUserCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.User)]
        [HttpGet("delete/{id:guid}")]
        [SwaggerOperation(
            Summary = "Delete user",
            Description = "Deletes user")]
        [SwaggerResponse(StatusCodes.Status200OK, "User deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new DeleteUserCommand { Id = id }));
        }

        [CustomAuthorize(Roles.User)]
        [HttpPatch("change-password")]
        [SwaggerOperation(
            Summary = "Change user password",
            Description = "Changes password of an user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password changed")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordCommand request)
        {
            return Ok(await Mediator.Send(request));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-all")]
        [SwaggerOperation(
            Summary = "Get all users",
            Description = "Gets a list of all users")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(ListResult<GetUsersListResponse>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> GetUsersList()
        {
            return Ok(await Mediator.Send(new GetUsersListQuery()));
        }

        [CustomAuthorize(Roles.Admin)]
        [HttpGet("get-paged", Name = GetPaged)]
        [SwaggerOperation(
         Summary = "Get paged users",
         Description = "Gets a paged list of users")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(PagedListResult<GetUsersListResponse>))]
        public async Task<IActionResult> GetPagedUsersList([FromQuery] GetPagedUsersListQuery request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
