namespace AdsPortal.Application.Operations.AuthorOperations.Queries.GetAuthorDetails
{
    using System;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;
    using Domain.Entities;

    public class GetAuthorDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string ORCID { get; set; } = string.Empty;

        public Guid DegreeId { get; set; }
        public string Degree { get; set; } = string.Empty;

        public Guid DepartmentId { get; set; }
        public string Department { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Author, GetAuthorDetailsResponse>()
                .ForMember(dest => dest.Degree, cfg => cfg.MapFrom(src => src.Degree.Name))
                .ForMember(dest => dest.Department, cfg => cfg.MapFrom(src => src.Department.Name));
        }
    }
}
