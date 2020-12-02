namespace AdsPortal.WebAPI.Controllers
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AuthenticationOperations.Commands.ResetPassword;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetResetPasswordToken;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/user")]
    [SwaggerTag("User authentication and password reset")]
    public class AuthenticationController : BaseController
    {
        public const string Login = nameof(Login);
        public const string ResetPasswordStep1 = nameof(ResetPasswordStep1);
        public const string ResetPasswordStep2 = nameof(ResetPasswordStep2);

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Login a user",
            Description = "Authenticates a user")]
        [SwaggerResponse(StatusCodes.Status200OK, "User authenticated", typeof(JwtTokenModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> LoginUser([FromBody] GetAuthenticationTokenQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("reset-password")]
        [SwaggerOperation(
            Summary = "Send password reset link (Reset password step 1)",
            Description = "Sends e-mail with password reset link")]
        [SwaggerResponse(StatusCodes.Status200OK, "Password reset e-mail sent")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ResetUserPasswordStep1([FromBody] SendResetPasswordTokenQuery request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPost("reset-password/change")]
        [SwaggerOperation(
            Summary = "Reset user password (Reset password step 2)",
            Description = "Resets user's password using password reset token")]
        [SwaggerResponse(StatusCodes.Status200OK, "User password was changed")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null, typeof(ExceptionResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, null, typeof(ExceptionResponse))]
        public async Task<IActionResult> ResetuserPasswordStep2([FromBody] ResetPasswordCommand request)
        {
            return Ok(await Mediator.Send(request));
        }
    }
}
