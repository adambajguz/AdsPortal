namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Commands.CreateAdvertisement
{
    using System;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Commands;

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

        private sealed class Handler : CreateCommandHandler<CreateAdvertisementCommand, CreateAdvertisementValidator, Advertisement>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            //TODO: add id validation for category, cover and author
        }
    }
}
