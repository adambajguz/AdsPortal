namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Domain.Models.MediaItem;
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

            protected override async ValueTask OnValidate(CancellationToken cancellationToken)
            {
                MediaItemAccessConstraintsModel constraints = await Uow.MediaItems.GetConstraintsAsync(Query.Id, cancellationToken);

                if (constraints.OwnerId != null)
                {
                   await _drs.IsOwnerOrCreatorOrAdminElseThrowAsync(constraints, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(constraints.Role);
            }

            protected override async ValueTask<GetMediaItemFileResponse> OnFetch(CancellationToken cancellationToken)
            {
                byte[]? fileContent = await _ifs.GetFileAsync("MediaItemCache", Query.Id, "file", cancellationToken);

                var mediaItem = await base.OnFetch(cancellationToken);

                if (fileContent is not null)
                {
                    mediaItem = mediaItem with { Data = fileContent };
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
        }
    }
}
