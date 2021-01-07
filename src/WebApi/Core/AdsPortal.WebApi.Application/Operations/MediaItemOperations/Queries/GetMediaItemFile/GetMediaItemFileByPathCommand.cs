namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Utils;
    using AutoMapper;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemFileByPathCommand : IGetDetailsQuery<GetMediaItemFileResponse>
    {
        public string Path { get; init; } = string.Empty;


        private class Handler : GetDetailsQueryHandler<GetMediaItemFileByPathCommand, MediaItem, GetMediaItemFileResponse>
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
        }
    }
}
