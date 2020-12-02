namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class GetAdvertisementsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

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
            configuration.CreateMap<Advertisement, GetAdvertisementsListResponse>();
        }
    }
}
