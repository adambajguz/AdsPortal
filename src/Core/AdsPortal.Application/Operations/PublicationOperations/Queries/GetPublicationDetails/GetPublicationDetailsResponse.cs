namespace AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationDetails
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;
    using Domain.Entities;

    public class GetPublicationDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public ushort InternalAuthors { get; set; }
        public ushort ExternalAuthors { get; set; }

        public double PointsToEvaluation { get; set; }
        public double SlotsToEvaluation { get; set; }

        public GetPublicationDetailsJournalResponse Journal { get; set; } = default!;
        public ICollection<GetPublicationDetailsAuthorResponse> PublicationAuthors { get; set; } = new List<GetPublicationDetailsAuthorResponse>();

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Publication, GetPublicationDetailsResponse>()
                         .ForMember(src => src.InternalAuthors, cfg => cfg.MapFrom(src => (ushort)src.PublicationAuthors.Count));
        }
    }

    public class GetPublicationDetailsAuthorResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<PublicationAuthor, GetPublicationDetailsAuthorResponse>()
                         .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => src.Author.Name))
                         .ForMember(dest => dest.Surname, cfg => cfg.MapFrom(src => src.Author.Surname));
        }
    }

    public class GetPublicationDetailsJournalResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Points { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Journal, GetPublicationDetailsJournalResponse>();
        }
    }
}
