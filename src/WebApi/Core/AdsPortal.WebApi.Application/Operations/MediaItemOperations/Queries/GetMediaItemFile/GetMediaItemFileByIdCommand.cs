namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemFileByIdCommand : IGetDetailsQuery<GetMediaItemFileResponse>, IIdentifiableOperation<GetMediaItemFileResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetDetailsByIdQueryHandler<GetMediaItemFileByIdCommand, MediaItem, GetMediaItemFileResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override ValueTask OnValidate(MediaItem entity, CancellationToken cancellationToken)
            {
                if (entity.OwnerId != null)
                {
                    _drs.IsOwnerOrCreatorOrAdminElseThrow(entity, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(entity.Role);

                return default;
            }
        }
    }
}
