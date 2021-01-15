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
    using AdsPortal.WebApi.Domain.Models.MediaItem;
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

            protected override async ValueTask<GetMediaItemChecksumResponse> OnFetch(CancellationToken cancellationToken)
            {
                string path = Query.Path;
                string fileName = System.IO.Path.GetFileName(path);
                string directory = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/') ?? string.Empty;

                long pathHashCode = MediaItemPathHasher.CalculatePathHash(path);

                return await Uow.MediaItems.ProjectedSingleAsync<GetMediaItemChecksumResponse>(x => x.PathHashCode == pathHashCode &&
                                                                                                    x.FileName == fileName &&
                                                                                                    x.VirtualDirectory == directory,
                                                                                               noTracking: true,
                                                                                               cancellationToken: cancellationToken);
            }

            protected override async ValueTask<GetMediaItemChecksumResponse> OnFetched(GetMediaItemChecksumResponse response, CancellationToken cancellationToken)
            {
                MediaItemAccessConstraintsModel constraints = await Repository.ProjectedSingleByIdAsync<MediaItemAccessConstraintsModel>(response.Id, true, cancellationToken);

                if (constraints.OwnerId != null)
                {
                    await _drs.IsOwnerOrCreatorOrAdminElseThrowAsync(constraints, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(constraints.Role);

                return response with
                {
                    FileName = response.FileName + ".sha512",
                    ContentType = MediaTypeNames.Text.Plain,
                    FileContent = FileUtils.GetChecksumFileContent(response.Hash, response.FileName)
                };
            }
        }
    }
}
