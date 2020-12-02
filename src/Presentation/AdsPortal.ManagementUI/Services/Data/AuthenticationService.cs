namespace AdsPortal.ManagementUI.Services.Data
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AuthenticationOperations.Commands.ResetPassword;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetResetPasswordToken;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using MediatR;

    public class AuthenticationService
    {
        private IMediator Mediator { get; }

        public AuthenticationService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<JwtTokenModel> LoginUser(string? email, string? password)
        {
            GetAuthenticationTokenQuery query = new GetAuthenticationTokenQuery
            {
                Email = email,
                Password = password
            };

            return await Mediator.Send(query);
        }

        public async Task ResetUserPasswordStep1(SendResetPasswordTokenQuery request)
        {
            await Mediator.Send(request);
        }

        public async Task<Unit> ResetuserPasswordStep2(ResetPasswordCommand request)
        {
            return await Mediator.Send(request);
        }
    }
}
