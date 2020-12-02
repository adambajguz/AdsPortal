namespace AdsPortal.Domain.Models.AuthorImporter
{
    using AutoMapper;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class AuthorsImporterRecord : ICustomMapping
    {
        public int No { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string ORCID { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<AuthorsImporterRecord, Author>()
                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                         .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                         .ForMember(dest => dest.ORCID, opt => opt.MapFrom(src => src.ORCID))
                         .ForAllOtherMembers(cfg => cfg.Ignore());
        }
    }
}
