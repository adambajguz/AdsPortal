namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Domain.Models.MediaItem;
    using AdsPortal.WebApi.Domain.Utils;
    using AutoMapper;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemDetailsByPathQuery : IGetDetailsQuery<GetMediaItemDetailsResponse>
    {
        public string Path { get; init; } = string.Empty;

        private sealed class Handler : GetDetailsQueryHandler<GetMediaItemDetailsByPathQuery, MediaItem, GetMediaItemDetailsResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override async ValueTask<GetMediaItemDetailsResponse> OnFetch(CancellationToken cancellationToken)
            {
                string path = Query.Path;
                string fileName = System.IO.Path.GetFileName(path);
                string directory = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/') ?? string.Empty;

                long pathHashCode = MediaItemPathHasher.CalculatePathHash(path);

                return await Uow.MediaItems.ProjectedSingleAsync<GetMediaItemDetailsResponse>(x => x.PathHashCode == pathHashCode &&
                                                                                                   x.FileName == fileName &&
                                                                                                   x.VirtualDirectory == directory,
                                                                                              noTracking: true,
                                                                                              cancellationToken: cancellationToken);
            }

            protected override async ValueTask<GetMediaItemDetailsResponse> OnFetched(GetMediaItemDetailsResponse response, CancellationToken cancellationToken)
            {
                MediaItemAccessConstraintsModel constraints = await Uow.MediaItems.GetConstraintsAsync(response.Id, cancellationToken);

                if (constraints.OwnerId != null)
                {
                    await _drs.IsOwnerOrCreatorOrAdminElseThrowAsync(constraints, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(constraints.Role);

                return response;
            }
        }
    }
}

