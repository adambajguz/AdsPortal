namespace AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalDetails
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using Domain.Entities;

    public class GetJournalDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ISSN { get; set; } = string.Empty;
        public string EISSN { get; set; } = string.Empty;

        public double Points { get; set; }

        public IList<GetJournalDetailsPublicationResponse> Publications { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Journal, GetJournalDetailsResponse>();
        }
    }

    public class GetJournalDetailsPublicationResponse : IIdentifiableOperation, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public ushort Year { get; set; } = default!;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Publication, GetJournalDetailsResponse>();
        }
    }
}
