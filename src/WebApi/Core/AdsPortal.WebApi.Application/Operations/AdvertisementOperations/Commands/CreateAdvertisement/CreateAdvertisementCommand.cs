namespace AdsPortal.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
{
    using System;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;

    public sealed record CreateAdvertisementCommand : ICreateCommand
    {
        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<CreateAdvertisementCommand, Advertisement>();
        }

        private class Handler : CreateCommandHandler<CreateAdvertisementCommand, CreateAdvertisementValidator, Advertisement>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            //TODO: add id validation for category, cover and author
        }
    }
}
