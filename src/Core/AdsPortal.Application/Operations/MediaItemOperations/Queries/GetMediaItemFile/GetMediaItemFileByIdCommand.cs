namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails;
    using AdsPortal.Domain.Entities;

    public class GetMediaItemFileByIdCommand : IGetDetailsByIdQuery<GetMediaItemFileResponse>
    {
        public Guid Id { get; }

        public GetMediaItemFileByIdCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetMediaItemFileByIdCommand, MediaItem, GetMediaItemFileResponse>
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
        }
    }
}
