namespace AdsPortal.Application.Operations.CategoryOperations.Queries.GetCategoriesList
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class GetCategoriesListResponse : IIdentifiableOperationResult, ICustomMapping
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
