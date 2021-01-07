namespace AdsPortal.Application.Operations.AdvertisementOperations.Commands.UpdateAdvertisement
{
    using System;
    using AdsPortal.Application.GenericHandlers.Relational.Commands;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Commands;
    using MediatR.GenericOperations.Mapping;
    using Newtonsoft.Json;

    public sealed record UpdateAdvertisementCommand : IUpdateCommand
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<UpdateAdvertisementCommand, Advertisement>();
        }

        private class Handler : UpdateCommandHandler<UpdateAdvertisementCommand, UpdateAdvertisementValidator, Advertisement>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }
        }
    }
}
