namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemFile
{
    using System;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;

    public sealed record GetMediaItemFileResponse : IIdentifiableOperationResult, ICustomMapping
    {
        public Guid Id { get; init; }

        public string FileName { get; init; } = string.Empty;
        public string VirtualDirectory { get; set; } = string.Empty;
        public byte[]? Data { get; init; }
        public string ContentType { get; init; } = string.Empty;
        public string Hash { get; init; } = string.Empty;

        void ICustomMapping.CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MediaItem, GetMediaItemFileResponse>()
                         .ForMember(dest => dest.Data, opt => opt.Ignore());
        }
    }
}
