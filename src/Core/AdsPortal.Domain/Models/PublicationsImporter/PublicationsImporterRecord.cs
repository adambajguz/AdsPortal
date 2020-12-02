namespace AdsPortal.Domain.Models.AuthorImporter
{
    using System;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;
    using AutoMapper;

    public class PublicationsImporterRecord : ICustomMapping
    {
        public int No { get; set; }

        public string[] Authors { get; set; } = Array.Empty<string>();
        public string Title { get; set; } = string.Empty;
        public ushort Year { get; set; }
        public string Journal { get; set; } = string.Empty;
        public string ListedAuthors { get; set; } = string.Empty;
        public string TotalAuthors { get; set; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<PublicationsImporterRecord, Publication>()
                         .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                         .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                         .ForMember(dest => dest.ExternalAuthors, opt => opt.MapFrom(src => src.TotalAuthors))
                         .ForAllOtherMembers(cfg => cfg.Ignore());
        }
    }
}
