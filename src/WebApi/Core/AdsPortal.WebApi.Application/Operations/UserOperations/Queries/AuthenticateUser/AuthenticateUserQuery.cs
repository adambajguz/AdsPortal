namespace AdsPortal.Application.Operations.UserOperations.Queries.AuthenticateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using AdsPortal.WebApi.Domain.Entities;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public sealed record AuthenticateUserQuery : IOperation<AuthenticateUserResponse>
    {
        public string? Email { get; init; }
        public string? Password { get; init; }

        private sealed class Handler : IRequestHandler<AuthenticateUserQuery, AuthenticateUserResponse>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IJwtService _jwt;
            private readonly IUserManagerService _userManager;

            public Handler(IAppRelationalUnitOfWork uow, IJwtService jwt, IUserManagerService userManager)
            {
                _uow = uow;
                _jwt = jwt;
                _userManager = userManager;
            }

            public async Task<AuthenticateUserResponse> Handle(AuthenticateUserQuery query, CancellationToken cancellationToken)
            {
                User user = await _uow.Users.SingleAsync(x => x.Email.Equals(query.Email), noTracking: true, cancellationToken);
                AuthenticateUserValidator.Model validationModel = new AuthenticateUserValidator.Model(query, user);

                await new AuthenticateUserValidator(_userManager).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                return _jwt.GenerateJwtToken(user);
            }
        }
    }
}
