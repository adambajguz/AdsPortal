namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails
{
    using System;
    using AutoMapper;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Jwt;
    using AdsPortal.Domain.Mapping;

    public class GetMediaItemDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;

        public string VirtualDirectory { get; set; } = string.Empty;

        public byte[]? Data { get; set; }
        public string Hash { get; set; } = string.Empty;
        public long ByteSize { get; set; }

        public Guid? OwnerId { get; set; }
        public Roles Role { get; set; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemDetailsResponse>();
        }
    }
}
