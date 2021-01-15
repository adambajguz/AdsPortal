namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemChecksumFile
{
    using System;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Utils;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Domain.Models.MediaItem;
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

            protected override async ValueTask OnValidate(CancellationToken cancellationToken)
            {
                MediaItemAccessConstraintsModel constraints = await Uow.MediaItems.GetConstraintsAsync(Query.Id, cancellationToken);

                if (constraints.OwnerId != null)
                {
                    await _drs.IsOwnerOrCreatorOrAdminElseThrowAsync(constraints, x => x.OwnerId);
                }

                _drs.HasRoleElseThrow(constraints.Role);
            }

            protected override ValueTask<GetMediaItemChecksumResponse> OnFetched(GetMediaItemChecksumResponse response, CancellationToken cancellationToken)
            {
                return ValueTask.FromResult(response with
                {
                    FileName = response.FileName + ".sha512",
                    ContentType = MediaTypeNames.Text.Plain,
                    FileContent = FileUtils.GetChecksumFileContent(response.Hash, response.FileName)
                });
            }
        }
    }
}
