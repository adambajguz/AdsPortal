namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemDetails
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class GetMediaItemDetailsResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string FileName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Alt { get; init; } = string.Empty;

        public string VirtualDirectory { get; init; } = string.Empty;

        public byte[]? Data { get; init; }
        public string Hash { get; init; } = string.Empty;
        public long ByteSize { get; init; }

        public Guid? OwnerId { get; init; }
        public Roles Role { get; init; }

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemDetailsResponse>();
        }
    }
}
