namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
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
            private readonly IIdentifiableFileStorageService _ifs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs, IIdentifiableFileStorageService ifs) : base(uow, mapper)
            {
                _drs = drs;
                _ifs = ifs;
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

            protected override async ValueTask<MediaItem> OnFetch(CancellationToken cancellationToken)
            {
                byte[]? fileContent = await _ifs.GetFileAsync("MediaItemCache", Query.Id, "file", cancellationToken);

                var mediaItem = await base.OnFetch(cancellationToken);

                if (fileContent is not null)
                {
                    mediaItem.Data = fileContent;
                }
                else if (mediaItem.Data is not null)
                {
                    await _ifs.SaveFileAsync("MediaItemCache", Query.Id, "file", mediaItem.ContentType, mediaItem.Data, cancellationToken);
                }

                //TODO: table splitting or explicit select without Data
                //https://entityframeworkcore.com/knowledge-base/49470417/ef-core-2-0---2-1---------------------
                //https://github.com/dotnet/efcore/issues/11708

                return mediaItem;
            }

            protected override ValueTask<GetMediaItemFileResponse> OnMapped(MediaItem entity, GetMediaItemFileResponse response, CancellationToken cancellationToken)
            {
                return base.OnMapped(entity, response, cancellationToken);
            }
        }
    }
}
