namespace AdsPortal.Application.Operations.DepartmentOperations.Commands.DeleteDepartment
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class DeleteDepartmentCommand : IDeleteByIdCommand
    {
        public Guid Id { get; }

        public DeleteDepartmentCommand(Guid id)
        {
            Id = id;
        }

        private class Handler : DeleteByIdHandler<DeleteDepartmentCommand, Department>
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
