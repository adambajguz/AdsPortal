namespace AdsPortal.Application.Operations.PublicationOperations.Commands.DeletePublication
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using Domain.Entities;

    public class DeletePublicationCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeletePublicationCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeletePublicationCommand, Publication>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
