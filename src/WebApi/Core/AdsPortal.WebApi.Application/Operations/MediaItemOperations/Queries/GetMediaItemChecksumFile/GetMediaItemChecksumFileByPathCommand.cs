namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemChecksumFile
{
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Application.Utils;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Utils;
    using AutoMapper;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemChecksumFileByPathCommand : IGetDetailsQuery<GetMediaItemChecksumResponse>
    {
        public string Path { get; init; } = string.Empty;

        private sealed class Handler : GetDetailsQueryHandler<GetMediaItemChecksumFileByPathCommand, MediaItem, GetMediaItemChecksumResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override ValueTask OnValidate(MediaItem entity, CancellationToken cancellationToken)
            {
                if (entity.OwnerId != null)
                    _drs.IsOwnerOrCreatorOrAdminElseThrow(entity, x => x.OwnerId);

                _drs.HasRoleElseThrow(entity.Role);

                return default;
            }

            protected override async ValueTask<MediaItem> OnFetch(CancellationToken cancellationToken)
            {
                string path = Query.Path;
                string fileName = System.IO.Path.GetFileName(path);
                string directory = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/') ?? string.Empty;

                long pathHashCode = MediaItemPathHasher.CalculatePathHash(path);

                return await Uow.MediaItems.SingleAsync(x => x.PathHashCode == pathHashCode &&
                                                             x.FileName == fileName &&
                                                             x.VirtualDirectory == directory,
                                                        noTracking: true,
                                                        cancellationToken: cancellationToken);
            }

            protected override ValueTask<GetMediaItemChecksumResponse> OnMapped(MediaItem entity, GetMediaItemChecksumResponse response, CancellationToken cancellationToken)
            {
                return ValueTask.FromResult(response with
                {
                    FileName = response.FileName + ".sha512",
                    ContentType = MediaTypeNames.Text.Plain,
                    FileContent = FileUtils.GetChecksumFileContent(entity.Hash, entity.FileName)
                });
            }
        }
    }
}
