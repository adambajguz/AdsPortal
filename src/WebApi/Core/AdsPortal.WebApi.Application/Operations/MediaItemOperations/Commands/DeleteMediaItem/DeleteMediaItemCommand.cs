namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Commands.DeleteMediaItem
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

    public sealed record DeleteMediaItemCommand : IDeleteCommand, IIdentifiableOperation
    {
        public Guid Id { get; init; }

        private sealed class Handler : DeleteByIdHandler<DeleteMediaItemCommand, MediaItem>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IDataRightsService drs) : base(uow)
            {
                _drs = drs;
            }

            protected override async ValueTask OnValidate(MediaItem entity, CancellationToken cancellationToken)
            {
                if (entity.OwnerId != null)
                {
                    await _drs.IsOwnerOrCreatorOrAdminElseThrowAsync(entity, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(entity.Role);
            }
        }
    }
}
