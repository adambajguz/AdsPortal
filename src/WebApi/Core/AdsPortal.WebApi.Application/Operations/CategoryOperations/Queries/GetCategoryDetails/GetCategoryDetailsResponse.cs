namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Queries.GetCategoryDetails
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public sealed record GetCategoryDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public IReadOnlyList<GetCategoryAdvertisementsDetailsResponse> Advertisements { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Category, GetCategoryDetailsResponse>();
        }
    }

    public sealed record GetCategoryAdvertisementsDetailsResponse : IIdentifiableOperation, ICustomMapping
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Advertisement, GetCategoryAdvertisementsDetailsResponse>();
        }
    }
}
