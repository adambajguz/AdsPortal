namespace AdsPortal.Domain.Models.JournalsImporter
{
    using System.Collections.Generic;
    using AutoMapper;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Mapping;

    public class JournalsImporterRecord : ICustomMapping
    {
        public int No { get; set; }
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string NameAlt { get; set; } = string.Empty;
        public string ISSN { get; set; } = string.Empty;
        public string EISSN { get; set; } = string.Empty;

        public double Points { get; set; }
        public List<string> Disciplines { get; set; } = new List<string>();

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<JournalsImporterRecord, Journal>()
                         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                         .ForMember(dest => dest.NameAlt, opt => opt.MapFrom(src => src.NameAlt))
                         .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
