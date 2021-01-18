namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.SendResetPasswordToken
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Jobs;
    using AdsPortal.WebApi.Domain.EmailTemplates;
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
            private readonly IJobSchedulingService _jobScheduling;
            private readonly IAppRelationalUnitOfWork _uow;
            private readonly IHttpContextAccessor _context;
            private readonly IJwtService _jwt;

            public SendResetPasswordTokenHandler(IJobSchedulingService jobScheduling, IAppRelationalUnitOfWork uow, IHttpContextAccessor context, IJwtService jwt)
            {
                _jobScheduling = jobScheduling;
                _uow = uow;
                _context = context;
                _jwt = jwt;
            }

            public async Task<Unit> Handle(SendResetPasswordTokenQuery query, CancellationToken cancellationToken)
            {
                await new SendResetPasswordTokenValidator().ValidateAndThrowAsync(query, cancellationToken: cancellationToken);

                User user = await _uow.Users.SingleAsync(x => x.Email.Equals(query.Email), noTracking: true, cancellationToken);

                string token = _jwt.GenerateJwtToken(user, Roles.ResetPassword).Token;
                Uri uri = _context.HttpContext?.GetAbsoluteUri() ?? throw new NullReferenceException("HttpContext is null");

                SendEmailJobArguments args = new()
                {
                    Email = user.Email,
                    Template = new ResetPasswordEmail { Name = user.Name, CallbackUrl = uri.AbsoluteUri + "/" + token }
                };

                await _jobScheduling.ScheduleAsync<SendEmailJob>(operationArguments: args, cancellationToken: cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
