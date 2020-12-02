namespace AdsPortal.Application.Operations.DegreeOperations.Queries.GetDegreesList
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetDegreesListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Degree, GetDegreesListResponse>();
        }
    }
}
