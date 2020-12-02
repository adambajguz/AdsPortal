namespace AdsPortal.Application.Operations.MediaItemOperations.Commands.DeleteUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class DeleteMediaItemCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeleteMediaItemCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeleteMediaItemCommand, MediaItem>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IDataRightsService drs) : base(uow)
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
