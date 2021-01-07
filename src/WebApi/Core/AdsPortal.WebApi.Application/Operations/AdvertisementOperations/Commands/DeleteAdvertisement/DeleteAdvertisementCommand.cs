namespace AdsPortal.Application.Operations.AdvertisementOperations.Commands.DeleteAdvertisement
{
    using System;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Commands;

    public sealed record DeleteAdvertisementCommand : IDeleteCommand, IIdentifiableOperation
    {
        public Guid Id { get; init; }

        private sealed class Handler : DeleteByIdHandler<DeleteAdvertisementCommand, Advertisement>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
