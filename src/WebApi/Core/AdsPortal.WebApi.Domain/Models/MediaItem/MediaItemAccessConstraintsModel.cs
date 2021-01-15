namespace AdsPortal.WebApi.Domain.Models.MediaItem
{
    using System;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using AutoMapper;
    using AutoMapper.Extensions;

    public record MediaItemAccessConstraintsModel : IEntityCreation, ICustomMapping
    {
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public Guid? OwnerId { get; set; }
        public Roles Role { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, MediaItemAccessConstraintsModel>();
        }
    }
}
