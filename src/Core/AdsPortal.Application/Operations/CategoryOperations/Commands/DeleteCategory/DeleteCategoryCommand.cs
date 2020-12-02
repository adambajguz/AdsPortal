namespace AdsPortal.Application.Operations.CategoryOperations.Commands.DeleteCategory
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using Domain.Entities;

    public class DeleteCategoryCommand : IDeleteByIdCommand
    {
        public Guid Id { get; init; }

        private class Handler : DeleteByIdHandler<DeleteCategoryCommand, Category>
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
