namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorDetails
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using Domain.Entities;

    public class GetPublicationAuthorDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public Guid? AuthorId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public Guid? PublicationId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<PublicationAuthor, GetPublicationAuthorDetailsResponse>()
                         .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => src.Author.Name))
                         .ForMember(dest => dest.Surname, cfg => cfg.MapFrom(src => src.Author.Surname))
                         .ForMember(dest => dest.Title, cfg => cfg.MapFrom(src => src.Publication.Title))
                         .ForMember(dest => dest.Year, cfg => cfg.MapFrom(src => src.Publication.Year));
        }
    }
}
