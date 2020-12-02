namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Commands.DeletePublicationAuthor
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using Domain.Entities;

    public class DeletePublicationAuthorCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeletePublicationAuthorCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeletePublicationAuthorCommand, PublicationAuthor>
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
