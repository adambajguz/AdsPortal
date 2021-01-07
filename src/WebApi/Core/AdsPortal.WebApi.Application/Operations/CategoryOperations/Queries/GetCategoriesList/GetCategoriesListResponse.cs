namespace AdsPortal.Application.Operations.CategoryOperations.Queries.GetCategoriesList
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public sealed record GetCategoriesListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Category, GetCategoriesListResponse>();
        }
    }
}
