namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemChecksumFile
{
    using System;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.Extensions;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails;
    using AdsPortal.Domain.Entities;

    public class GetMediaItemChecksumFileByIdCommand : IGetDetailsByIdQuery<GetMediaItemChecksumResponse>
    {
        public Guid Id { get; }

        public GetMediaItemChecksumFileByIdCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetMediaItemChecksumFileByIdCommand, MediaItem, GetMediaItemChecksumResponse>
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
