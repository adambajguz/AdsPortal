namespace AdsPortal.Application.Operations.JournalOperations.Commands.DeleteJournal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using Domain.Entities;

    public class DeleteJournalCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeleteJournalCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeleteJournalCommand, Journal>
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
