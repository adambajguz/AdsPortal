namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.Domain.Mapping;

    public class GetMediaItemsListResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;

        public string VirtualDirectory { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long ByteSize { get; set; }

        public Guid? OwnerId { get; set; }
        public Roles Role { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemsListResponse>();
        }
    }
}
