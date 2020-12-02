namespace AdsPortal.Application.Operations.AuthorOperations.Commands.DeleteAuthor
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class DeleteAuthorCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeleteAuthorCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeleteAuthorCommand, Author>
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
