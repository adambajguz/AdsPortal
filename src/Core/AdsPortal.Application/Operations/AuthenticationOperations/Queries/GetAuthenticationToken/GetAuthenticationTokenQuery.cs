namespace AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using FluentValidation;
    using MediatR;

    public class GetAuthenticationTokenQuery : IOperation<JwtTokenModel>
    {
        public string? Email { get; init; }
        public string? Password { get; init; }

        private class Handler : IRequestHandler<GetAuthenticationTokenQuery, JwtTokenModel>
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

            public async Task<JwtTokenModel> Handle(GetAuthenticationTokenQuery query, CancellationToken cancellationToken)
            {
                User user = await _uow.Users.SingleAsync(x => x.Email.Equals(query.Email), noTracking: true, cancellationToken);
                GetAuthenticationTokenValidator.Model validationModel = new GetAuthenticationTokenValidator.Model(query, user);

                await new GetAuthenticationTokenValidator(_userManager).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                return _jwt.GenerateJwtToken(user);
            }
        }
    }
}
