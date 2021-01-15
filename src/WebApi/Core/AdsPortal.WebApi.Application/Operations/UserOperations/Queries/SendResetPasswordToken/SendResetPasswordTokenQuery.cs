namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.SendResetPasswordToken
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Extensions;
    using AdsPortal.WebApi.Application.Interfaces;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Domain.Jwt;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;
    using Microsoft.AspNetCore.Http;

    public sealed record SendResetPasswordTokenQuery : IOperation
    {
        public string? Email { get; init; }

        private sealed class SendResetPasswordTokenHandler : IRequestHandler<SendResetPasswordTokenQuery>
        {
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IHttpContextAccessor _context;
            private readonly IJwtService _jwt;
            private readonly IEmailService _email;

            public SendResetPasswordTokenHandler(IAppRelationalUnitOfWork uow, IHttpContextAccessor context, IJwtService jwt, IEmailService email)
            {
                _uow = uow;
                _context = context;
                _jwt = jwt;
                _email = email;
            }

            public async Task<Unit> Handle(SendResetPasswordTokenQuery query, CancellationToken cancellationToken)
            {
                await new SendResetPasswordTokenValidator().ValidateAndThrowAsync(query, cancellationToken: cancellationToken);

                User user = await _uow.Users.SingleAsync(x => x.Email.Equals(query.Email), noTracking: true, cancellationToken);

                string token = _jwt.GenerateJwtToken(user, Roles.ResetPassword).Token;
                Uri uri = _context.HttpContext?.GetAbsoluteUri() ?? throw new NullReferenceException("HttpContext is null");
                await _email.SendEmail(user.Email, "Reset Password", uri.AbsoluteUri + "/" + token);

                //#pragma warning disable CS0162 // Unreachable code detected
                //                if (GlobalAppConfig.IsDevMode)
                //                    return uri.AbsoluteUri + "/" + token;
                //#pragma warning restore CS0162 // Unreachable code detected

                //                return "e-mail sent";

                return await Unit.Task;
            }
        }
    }
}
