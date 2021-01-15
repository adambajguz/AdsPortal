namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.DeleteUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Commands;

    public sealed record DeleteUserCommand : IDeleteCommand, IIdentifiableOperation
    {
        public Guid Id { get; init; }

        private sealed class Handler : DeleteByIdHandler<DeleteUserCommand, User>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IDataRightsService drs) : base(uow)
            {
                _drs = drs;
            }

            protected override async ValueTask OnValidate(User entity, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrowAsync(Command.Id);
            }
        }
    }
}
