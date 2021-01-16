namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
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

            protected override async ValueTask<GetMediaItemFileResponse> OnFetched(GetMediaItemFileResponse response, CancellationToken cancellationToken)
            {
                Guid id = response.Id;
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
