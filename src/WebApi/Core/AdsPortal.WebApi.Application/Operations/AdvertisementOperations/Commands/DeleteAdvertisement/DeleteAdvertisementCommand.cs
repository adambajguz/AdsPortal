namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.DeleteAdvertisement
{
    using System;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
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
