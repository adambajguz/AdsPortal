namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.AuthenticateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Constants;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
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
                await new AuthenticateUserValidator().ValidateAndThrowAsync(query, cancellationToken: cancellationToken);

                User user = await _uow.Users.SingleAsync(x => x.Email.Equals(query.Email), noTracking: true, cancellationToken);

                if (!await _userManager.ValidatePassword(user, query.Password ?? string.Empty, cancellationToken))
                {
                    throw new ValidationFailedException(nameof(query.Email), ValidationMessages.Auth.EmailOrPasswordIsIncorrect);
                }

                return _jwt.GenerateJwtToken(user);
            }
        }
    }
}
