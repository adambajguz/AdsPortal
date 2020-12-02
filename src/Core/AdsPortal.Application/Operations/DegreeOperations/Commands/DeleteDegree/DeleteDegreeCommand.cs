namespace AdsPortal.Application.Operations.DegreeOperations.Commands.DeleteDegree
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class DeleteDegreeCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeleteDegreeCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeleteDegreeCommand, Degree>
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
