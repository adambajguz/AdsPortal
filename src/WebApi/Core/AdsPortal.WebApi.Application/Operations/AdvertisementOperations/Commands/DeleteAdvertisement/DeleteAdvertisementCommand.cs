namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.DeleteAdvertisement
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Commands;

    public sealed record DeleteAdvertisementCommand : IDeleteCommand, IIdentifiableOperation
    {
        public Guid Id { get; init; }

        private sealed class Handler : DeleteByIdHandler<DeleteAdvertisementCommand, Advertisement>
        {
            private readonly IDataRightsService _drs;

            public Handler(IDataRightsService drs, IAppRelationalUnitOfWork uow) : base(uow)
            {
                _drs = drs;
            }

            protected override async ValueTask OnValidate(Advertisement response, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrowAsync(response.AuthorId);
            }
        }
    }
}
