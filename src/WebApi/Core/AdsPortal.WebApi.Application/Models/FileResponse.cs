namespace AdsPortal.WebApi.Application.Models
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using AutoMapper.Extensions;

    public sealed record FileResponse : ICustomMapping
    {
        public Guid Id { get; init; }
        public string Path { get; init; } = string.Empty;
        public string? Alt { get; init; }
        public string? Title { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, FileResponse>()
                         .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.VirtualDirectory + "/" + src.FileName));
        }
    }
}
