namespace AdsPortal.Application.Operations.CategoryOperations.Commands.DeleteCategory
{
    using System;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Commands;

    public sealed record DeleteCategoryCommand : IDeleteCommand, IIdentifiableOperation
    {
        public Guid Id { get; init; }

        private class Handler : DeleteByIdHandler<DeleteCategoryCommand, Category>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
