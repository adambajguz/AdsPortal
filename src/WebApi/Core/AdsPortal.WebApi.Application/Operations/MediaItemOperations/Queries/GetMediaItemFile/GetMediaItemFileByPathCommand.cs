namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Domain.Models.MediaItem;
    using AdsPortal.WebApi.Domain.Utils;
    using AutoMapper;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemFileByPathCommand : IGetDetailsQuery<GetMediaItemFileResponse>
    {
        public string Path { get; init; } = string.Empty;

        private sealed class Handler : GetDetailsQueryHandler<GetMediaItemFileByPathCommand, MediaItem, GetMediaItemFileResponse>
        {
            private readonly IDataRightsService _drs;
            private readonly IIdentifiableFileStorageService _ifs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs, IIdentifiableFileStorageService ifs) : base(uow, mapper)
            {
                _drs = drs;
                _ifs = ifs;
            }

            protected override async ValueTask<GetMediaItemFileResponse> OnFetch(CancellationToken cancellationToken)
            {
                string path = Query.Path;
                string fileName = System.IO.Path.GetFileName(path);
                string directory = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/') ?? string.Empty;

                long pathHashCode = MediaItemPathHasher.CalculatePathHash(path);

                return await Uow.MediaItems.ProjectedSingleAsync<GetMediaItemFileResponse>(x => x.PathHashCode == pathHashCode &&
                                                                                                x.FileName == fileName &&
                                                                                                x.VirtualDirectory == directory,
                                                                                           noTracking: true,
                                                                                           cancellationToken: cancellationToken);
            }

            protected override async ValueTask<GetMediaItemFileResponse> OnFetched(GetMediaItemFileResponse response, CancellationToken cancellationToken)
            {
                Guid id = response.Id;
                MediaItemAccessConstraintsModel constraints = await Uow.MediaItems.GetConstraintsAsync(id, cancellationToken);

                if (constraints.OwnerId != null)
                {
                    await _drs.IsOwnerOrCreatorOrAdminElseThrowAsync(constraints, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(constraints.Role);

                byte[]? fileContent = await _ifs.GetFileAsync("MediaItemCache", id, "file", cancellationToken);
                byte[]? hashContent = await _ifs.GetFileAsync("MediaItemCache", id, "sha515", cancellationToken);

                if (fileContent is null || hashContent.AsString() != response.Hash)
                {
                    fileContent = await Uow.MediaItems.GetFileData(id, cancellationToken);

                    if (fileContent is not null)
                    {
                        await _ifs.SaveFileAsync("MediaItemCache", id, "file", response.ContentType, fileContent, cancellationToken);

                        hashContent = response.Hash.AsByteArray();
                        await _ifs.SaveFileAsync("MediaItemCache", id, "sha515", response.ContentType, hashContent, cancellationToken);
                    }
                }

                return response with { Data = fileContent };
            }
        }
    }
}
