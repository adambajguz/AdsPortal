namespace AdsPortal.Application.Operations.PublicationAuthorOperations.Queries.GetPublicationAuthorsList
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class GetPublicationAuthorsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string ORCID { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<PublicationAuthor, GetPublicationAuthorsListResponse>();
        }
    }
}
