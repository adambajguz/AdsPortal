namespace AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalsList
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class GetJournalsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ISSN { get; set; } = string.Empty;
        public string EISSN { get; set; } = string.Empty;

        public double Points { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Journal, GetJournalsListResponse>();
        }
    }
}
