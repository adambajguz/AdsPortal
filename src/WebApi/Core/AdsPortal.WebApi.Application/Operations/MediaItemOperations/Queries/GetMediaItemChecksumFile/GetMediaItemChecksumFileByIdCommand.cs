namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemChecksumFile
{
    using System;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Extensions;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemChecksumFileByIdCommand : IGetDetailsQuery<GetMediaItemChecksumResponse>, IIdentifiableOperation<GetMediaItemChecksumResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetDetailsByIdQueryHandler<GetMediaItemChecksumFileByIdCommand, MediaItem, GetMediaItemChecksumResponse>
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
