namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemChecksumFile
{
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Extensions;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Utils;
    using AutoMapper;

    public class GetMediaItemChecksumFileByPathCommand : IGetDetailsQuery<GetMediaItemChecksumResponse>
    {
        public string Path { get; }

        public GetMediaItemChecksumFileByPathCommand(string path)
        {
            Path = path;
        }

        private class Handler : GetDetailsQueryHandler<GetMediaItemChecksumFileByPathCommand, MediaItem, GetMediaItemChecksumResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override Task OnValidate(MediaItem entity, CancellationToken cancellationToken)
            {
                if (entity.OwnerId != null)
                    _drs.IsOwnerOrCreatorOrAdminElseThrow(entity, x => x.OwnerId);

                _drs.HasRoleElseThrow(entity.Role);

                return Task.CompletedTask;
            }

            protected override async Task<MediaItem> OnFetch(CancellationToken cancellationToken)
            {
                string path = Query.Path;
                string fileName = System.IO.Path.GetFileName(path);
                string directory = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/') ?? string.Empty;

                long pathHashCode = MediaItemPathHasher.CalculatePathHashCode(path);

                return await Uow.MediaItems.SingleAsync(x => x.PathHashCode == pathHashCode &&
                                                             x.FileName == fileName &&
                                                             x.VirtualDirectory == directory,
                                                        noTracking: true,
                                                        cancellationToken: cancellationToken);
            }

            protected override Task OnMapped(MediaItem entity, GetMediaItemChecksumResponse response, CancellationToken cancellationToken)
            {
                response.FileName += ".sha512";
                response.ContentType = MediaTypeNames.Text.Plain;
                response.FileContent = FileUtils.GetChecksumFileContent(entity.Hash, entity.FileName);

                return Task.CompletedTask;
            }
        }
    }
}
