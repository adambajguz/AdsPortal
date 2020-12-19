namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementDetails
{
    using System;
    using AutoMapper;
    using Domain.Entities;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetAdvertisementDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public virtual MediaItem? CoverImage { get; init; } = default!;

        public Guid CategoryId { get; init; }
        public Category Category { get; init; } = default!;

        public Guid AuthorId { get; init; }
        public User Author { get; init; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Advertisement, GetAdvertisementDetailsResponse>();
        }
    }
}
