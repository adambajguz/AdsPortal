namespace AdsPortal.Application.Operations.PublicationOperations.Queries.GetPublicationsList
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetPublicationsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public ushort InternalAuthors { get; set; }
        public ushort ExternalAuthors { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Publication, GetPublicationsListResponse>()
                         .ForMember(src => src.InternalAuthors, cfg => cfg.MapFrom(src => (ushort)src.PublicationAuthors.Count));
        }
    }
}
